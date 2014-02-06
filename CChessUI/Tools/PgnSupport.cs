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
using System.IO;
using System.Linq;
using System.Windows.Input;
using CChessCore;
using CChessCore.Pgn;
using CChessDatabase;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Win32;
using WpfTools.Dialogs;

namespace CChessUI.Tools
{
    [Export(typeof(ITool))]
    public class PgnSupport : NotificationObject, ITool, IDialogViewModel
    {
        private readonly IGameController _gameController;
        private readonly IDialogService _dialogService;
        private readonly ICChessDatabaseService _databaseContext;
        private IDictionary<GameInfo, PgnGame> _games;

        public string Name { get { return "PGN"; } }

        public ICommand OkCommand { get; private set; }

        public ICommand Command { get; private set; }

        public ICommand LoadPgnFileCommand { get; private set; }

        public ObservableCollection<GameInfo> PgnGames { get; private set; }

        public GameInfo SelectedGame { get; set; }

        public event EventHandler<RequestCloseEventArgs> RequestClose;

        /// <summary>
        /// Initializes a new instance of the <see cref="PgnSupport"/> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="dialogService">The dialog service.</param>
        [ImportingConstructor]
        public PgnSupport(IGameController gameController,
                          ICChessDatabaseService databaseContext,
                          IDialogService dialogService)
        {
            _dialogService = dialogService;
            _databaseContext = databaseContext;
            _gameController = gameController;
            _games = new Dictionary<GameInfo, PgnGame>();

            PgnGames = new ObservableCollection<GameInfo>();

            OkCommand = new DelegateCommand(OnOkCommandExecuted);

            Command = new DelegateCommand(OnLoadPgnExecuted);
            LoadPgnFileCommand = new DelegateCommand(OnLoadPgnFileExecuted);
        }

        private void OnOkCommandExecuted()
        {
            if (SelectedGame != null)
            {
                if(_games.ContainsKey(SelectedGame))
                {
                    PgnParser parser = new PgnParser();
                    _gameController.StartNewGame(parser.ConvertGame(_games[SelectedGame]));
                }
            }

            OnRequestClose(false);
        }

        private void OnLoadPgnFileExecuted()
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".pgn";
            dialog.Filter = "Chess Databases (.pgn)|*.pgn";

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                PgnParser parser = new PgnParser();
                parser.Load(new FileStream(filename, FileMode.Open));
                
                _games.Clear();
                foreach (var item in parser.GetGames())
                {
                    _games.Add(parser.GetGameInfo(item), item);
                }

                PgnGames.Clear();
                foreach(var item in _games.Keys)
                {
                    PgnGames.Add(item);
                }
            }
        }

        private void OnLoadPgnExecuted()
        {
            _dialogService.ShowDialog(typeof(LoadPgnDialog), this, OnPgnDialogClosed);
        }

        private void OnPgnDialogClosed(IDialogViewModel model, bool? dialogresult, object state)
        {
            if (dialogresult == true)
            {
                PgnParser parser = new PgnParser();
                parser.Load(Result);
                _gameController.StartNewGame(parser.GetGame());
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
