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
using System.ComponentModel.Composition;
using System.Windows.Input;
using CChessCore;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Utilities;
using WpfTools.Dialogs;

namespace CChessUI.Tools
{
    [Export(typeof(ITool))]
    public class FenSupport : NotificationObject, ITool, IDialogViewModel
    {
        private string _fenOfBoard;
        private readonly IDialogService _dialogService;
        private readonly IGameController _gameController;

        public string Name { get { return "FEN"; } }

        public string FenOfBoard
        {
            get { return _fenOfBoard; }
            set
            {
                _fenOfBoard = value;
                RaisePropertyChanged(() => FenOfBoard);
            }
        }

        public ICommand Command { get; private set; }

        public ICommand GetBoardCommand { get; private set; }

        public ICommand OkCommand { get; private set; }

        public event EventHandler<RequestCloseEventArgs> RequestClose;


        /// <summary>
        /// Initializes a new instance of the <see cref="FenSupport"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        /// <param name="dialogService">The dialog service.</param>
        [ImportingConstructor]
        public FenSupport(IGameController gameController, IDialogService dialogService)
        {
            Command = new DelegateCommand(OnLoadFenExcecuted);
            OkCommand = new DelegateCommand(() => OnRequestClose(true));
            GetBoardCommand = new DelegateCommand(OnGetBoardExecuted);
            _dialogService = dialogService;
            _gameController = gameController;
        }

        private void OnGetBoardExecuted()
        {
            FenOfBoard = Fen.GetFen(_gameController.Game, false);
        }

        private void OnLoadFenExcecuted()
        {
            _dialogService.ShowDialog(typeof(LoadFenDialog), this, OnFenDialogClosed);
        }

        private void OnFenDialogClosed(IDialogViewModel model, bool? dialogresult, object state)
        {
            if (dialogresult == true)
            {
                _gameController.StartNewGame(Fen.GetBoard(Result));
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
