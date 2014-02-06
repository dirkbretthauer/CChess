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
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
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
    public class OpenFromDatabaseSupport : NotificationObject, ITool, IDialogViewModel
    {
        private readonly IGameController _gameController;
        private readonly IDialogService _dialogService;
        private readonly ICChessDatabaseService _databaseContext;
        private GameInfo _selectedGame;

        public string Name { get { return "Database"; } }

        public ICommand OkCommand { get; private set; }

        public ICommand Command { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public ICommand ImportCommand {get; private set;}

        public ObservableCollection<GameInfo> Games { get; private set; }

        public GameInfo SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                RaisePropertyChanged(() => SelectedGame);
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
        public OpenFromDatabaseSupport(IGameController gameController,
                          ICChessDatabaseService databaseContext,
                          IDialogService dialogService)
        {
            _dialogService = dialogService;
            _databaseContext = databaseContext;
            _gameController = gameController;

            OkCommand = new DelegateCommand(() => OnRequestClose(true));
            DeleteCommand = new DelegateCommand(OnDeleteGameExceuted);
            Command = new DelegateCommand(OnOpenDatabaseExecuted);
            ImportCommand = new DelegateCommand(OnImportExecuted);

            Games = new ObservableCollection<GameInfo>();
        }

        private void OnImportExecuted()
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

                foreach (var item in parser.GetGames())
                {
                    _databaseContext.Save(item);
                }

                Games.Clear();
                var games = _databaseContext.GetAllGames();
                foreach (var game in games)
                {
                    Games.Add(game);
                }
            }
        }

        private void OnDeleteGameExceuted()
        {
            if (_selectedGame != null)
            {
                _databaseContext.RemoveGame(_selectedGame);
            }
        }

        private void OnOpenDatabaseExecuted()
        {
            Games.Clear();
            var games = _databaseContext.GetAllGames();
            foreach (var game in games)
            {
                Games.Add(game);
            }

            _dialogService.ShowDialog(typeof(DatabaseDialog), this, OnDatabaseDialogClosed);
        }

        private void OnDatabaseDialogClosed(IDialogViewModel model, bool? dialogresult, object state)
        {
            if (dialogresult == true && _selectedGame != null)
            {
                var game = _databaseContext.GetGame(_selectedGame);
                if (game != null)
                {
                    _gameController.StartNewGame(game);
                }
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
