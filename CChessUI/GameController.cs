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
using System.Linq;
using System.Text;
using CChessCore;
using CChessCore.Rules;

namespace CChessUI
{
    
    [Export(typeof(IGameController))]
    public class GameController : IGameController
    {
        public IGame Game {get; private set;}

        public event EventHandler NewGameStarted;

        public GameController()
        {
            
        }

        public void StartNewGame()
        {
            Game = null;
            Game = CChessCore.Game.CreateChessGame();
            OnNewGameStarted();
        }

        public void StartNewGame(IGame game)
        {
            Game = null;
            Game = game;
            Game.GoToBegin();
            OnNewGameStarted();
        }

        private void OnNewGameStarted()
        {
            EventRaiser.TryRaiseEvent(NewGameStarted, this, EventArgs.Empty);
        }
    }
}
