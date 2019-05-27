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
using CChessUI.ViewModels;
using Prism.Commands;
using Prism.Mvvm;

namespace CChessUI.ViewModels
{
    public class NavigationViewModel : BindableBase
    {
        private readonly IGameController _gameController;
        private IGame _game;

        public DelegateCommand MoveBackwardCommand { get; private set; }

        public DelegateCommand MoveForwardCommand { get; private set; }

        public DelegateCommand MoveBeginCommand { get; private set; }

        public DelegateCommand MoveEndCommand { get; private set; }

        [ImportingConstructor]
        public NavigationViewModel(IGameController gameController)
        {
            _gameController = gameController;

            MoveBackwardCommand = new DelegateCommand(OnMoveBackwardExceuted, CanMoveBackward);
            MoveForwardCommand = new DelegateCommand(OnMoveForwardExecuted, CanMoveForward);
            MoveBeginCommand = new DelegateCommand(() => _game.GoToBegin());
            MoveEndCommand = new DelegateCommand(() => _game.GoToEnd());

            gameController.NewGameStarted += OnNewGameStarted;
            if (gameController.Game != null)
            {
                _game = gameController.Game;
                _game.MoveListUpdated += OnMoveListUpdated;
            }
        }

        private void OnMoveListUpdated(object sender, MoveListChangedEventArgs args)
        {
            MoveBackwardCommand.RaiseCanExecuteChanged();
            MoveForwardCommand.RaiseCanExecuteChanged();
        }

        private void OnNewGameStarted(object sender, EventArgs e)
        {
            if (_game != null)
            {
                _game.MoveListUpdated -= OnMoveListUpdated;
            }

            _game = ((IGameController)sender).Game;
            _game.MoveListUpdated += OnMoveListUpdated;

            MoveBackwardCommand.RaiseCanExecuteChanged();
            MoveForwardCommand.RaiseCanExecuteChanged();
        }

        private bool CanMoveForward()
        {
            return _game != null && _game.Movelist.CurrentHalfMoveNumber < _game.Movelist.Count;
        }

        private void OnMoveForwardExecuted()
        {
            _gameController.Game.GoForward();
            MoveBackwardCommand.RaiseCanExecuteChanged();
            MoveForwardCommand.RaiseCanExecuteChanged();
        }

        private void OnMoveBackwardExceuted()
        {
            _gameController.Game.GoBackward();
            MoveBackwardCommand.RaiseCanExecuteChanged();
            MoveForwardCommand.RaiseCanExecuteChanged();
        }

        private bool CanMoveBackward()
        {
            return _game != null && _game.Movelist.CurrentHalfMoveNumber > 0;
        }
    }
}
