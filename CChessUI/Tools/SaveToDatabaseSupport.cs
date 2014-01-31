#region Header
////////////////////////////////////////////////////////////////////////////// 
//    This file is part of $projectname$.
//
//    $projectname$ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    $projectname$ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with $projectname$.  If not, see <http://www.gnu.org/licenses/>.
///////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CChessCore;
using CChessDatabase;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Utilities;
using WpfTools.Dialogs;

namespace CChessUI.Tools
{
    [Export(typeof(ITool))]
    public class SaveToDatabaseSupport : NotificationObject, ITool, IDialogViewModel
    {
        private readonly IGameController _gameController;
        private readonly IDialogService _dialogService;
        private readonly ICChessDatabaseService _databaseContext;
        private string _white;
        private string _black;
        private string _event;

        public string Name { get { return "Save"; } }

        public ICommand OkCommand { get; private set; }

        public ICommand Command { get; private set; }

        public string White
        {
            get { return _white; }
            set
            {
                _white = value;
                RaisePropertyChanged(() => White);
            }
        }

        public string Black
        {
            get { return _black; }
            set
            {
                _black = value;
                RaisePropertyChanged(() => Black);
            }
        }

        public string Event
        {
            get { return _event; }
            set
            {
                _event = value;
                RaisePropertyChanged(() => Event);
            }
        }


        public event EventHandler<RequestCloseEventArgs> RequestClose;


        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFromDatabaseSupport" /> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="dialogService">The dialog service.</param>
        [ImportingConstructor]
        public SaveToDatabaseSupport(IGameController gameController,
                          ICChessDatabaseService databaseContext,
                          IDialogService dialogService)
        {
            _dialogService = dialogService;
            _databaseContext = databaseContext;
            _gameController = gameController;

            OkCommand = new DelegateCommand(() => OnRequestClose(true));
            Command = new DelegateCommand(OnSaveToDatabaseExecuted);
        }

        private void OnSaveToDatabaseExecuted()
        {
            White = _gameController.Game.Info.White;
            Black = _gameController.Game.Info.Black;
            Event = _gameController.Game.Info.Event;

            _dialogService.ShowDialog(typeof(SaveGameDialog), this, OnSaveGameViewClosed);
        }

        private void OnSaveGameViewClosed(IDialogViewModel model, bool? dialogresult, object state)
        {
            if (dialogresult == true)
            {
                _gameController.Game.Info.White = White;
                _gameController.Game.Info.Black = Black;
                _gameController.Game.Info.Event = Event;
                _databaseContext.Save(_gameController.Game);
            }
        }

        private void OnRequestClose(bool result)
        {
            EventRaiser.TryRaiseEvent(RequestClose, this, new RequestCloseEventArgs(result));
        }

        /// <summary>
        /// Provides the user input of the dialog.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Provides the Result of the dialog.
        /// </summary>
        public object ResultObject { get; set; }
    }
}
