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
using CChessCore;
using CChessUI.ViewModels;
using Microsoft.Practices.Prism.ViewModel;

namespace CChessUI
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class NotationViewModel : NotificationObject
    {
        private MoveViewModel _selectedMove;
        private GameInfo _gameInfo;
        private string _result;
        private IGame _game;

        public ObservableCollection<MoveViewModel> MoveList { get; private set;}

        public MoveViewModel SelectedMove
        {
            get { return _selectedMove; }
            set
            {
                _selectedMove = value;

                if (_selectedMove != null)
                {
                    int halfMoveNumber = _selectedMove.IsWhiteMove
                                             ? ((_selectedMove.TurnNumber - 1)*2) + 1
                                             : ((_selectedMove.TurnNumber - 1)*2) + 2;
                    _game.GoToTurn(halfMoveNumber);

                    RaisePropertyChanged(() => SelectedMove);
                }
            }
        }

        public GameInfo GameInfo
        {
            get { return _gameInfo; }
            set
            {
                _gameInfo = value;
                RaisePropertyChanged(() => GameInfo);
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged(() => Result);
            }
        }

        [ImportingConstructor]
        public NotationViewModel(IGameController gameController)
        {
            MoveList = new ObservableCollection<MoveViewModel>();
            gameController.NewGameStarted += OnNewGameStarted;
            if(gameController.Game != null)
            {
                _game = gameController.Game;
                _game.MoveListUpdated += OnMoveListUpdated;

                GameInfo = _game.Info;
                Result = _game.Info.Result;
            }
        }

        private void OnMoveListUpdated(object sender, MoveListChangedEventArgs args)
        {
            if(args.Reason == MoveListChangeReason.Reset)
            {
                MoveList.Clear();
                foreach (var move in _game.Movelist)
                {
                    MoveList.Add(new MoveViewModel(move));
                }
            }

            if(args.Reason == MoveListChangeReason.Add)
            {
                MoveList.Insert(args.HalfMove - 1, new MoveViewModel(args.Move));
            }
        }

        private void OnNewGameStarted(object sender, EventArgs e)
        {
            if(_game != null)
            {
                _game.MoveListUpdated -= OnMoveListUpdated;
            }

            MoveList.Clear();
            _game = ((IGameController) sender).Game;
            foreach (var move in _game.Movelist)
            {
                MoveList.Add(new MoveViewModel(move));
            }
            _game.MoveListUpdated += OnMoveListUpdated;

            GameInfo = _game.Info;
            Result = _game.Info.Result;
        }
    }
}
