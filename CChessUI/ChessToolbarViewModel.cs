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
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CChessCore;
using CChessCore.Pgn;
using CChessDatabase;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Win32;
using WpfTools.Dialogs;

namespace CChessUI
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ChessToolbarViewModel : NotificationObject
    {
        private readonly IGameController _gameController;
        private readonly ICChessDatabaseService _databaseContext;
        private readonly IDialogService _dialogService;

        public ICommand NewGameCommand { get; private set; }

        public IEnumerable<ITool> Tools { get; private set; }

        [ImportingConstructor]
        public ChessToolbarViewModel(IGameController gameController,
                                     ICChessDatabaseService databaseContext,
                                     IDialogService dialogService,
                                     [ImportMany] IEnumerable<ITool> tools)
        {
            _gameController = gameController;
            _databaseContext = databaseContext;
            Tools = tools;

            NewGameCommand = new DelegateCommand(OnNewGameExecuted);

            _databaseContext.Load();
        }

        private void OnSaveGameDialogClosed(IDialogViewModel model, bool? dialogResult, object state)
        {
            _databaseContext.Save(_gameController.Game);
        }

        private void OnNewGameExecuted()
        {
            _gameController.StartNewGame();
        }
    }
}
