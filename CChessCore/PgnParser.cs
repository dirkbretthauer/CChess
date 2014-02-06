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

using CChessCore.Pgn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CChessCore
{
    public class PgnParser : BaseChessNotationParser
    {
        private PgnReader _reader;

        public void Load(string testData)
        {
            _reader = new PgnReader(new StringReader(testData), PgnReader.DefaultBufferSize);
        }

        public void Load(FileStream fileStream)
        {
            _reader = new PgnReader(new StreamReader(fileStream, Encoding.UTF8), PgnReader.DefaultBufferSize);
        }

        public IEnumerable<PgnGame> GetGames()
        {
            IList<PgnGame> result = new List<PgnGame>();
            while (_reader.ReadGame())
            {
                result.Add(_reader.CurrentGame);
            }

            return result;
        }

        public IGame ConvertGame(PgnGame game)
        {
            _currentGame = Game.CreateChessGame();
            
            foreach (var move in game.Moves.Moves)
            {
                DoMove(move.Move);
                _currentGame.AddComment(_currentGame.Movelist.CurrentHalfMoveNumber, move.Comment);
            }

            _currentGame.Info = GetGameInfo(game);

            return _currentGame;
        }

        public PgnMoves GetMoves()
        {
            _reader.ReadGame();
            return _reader.CurrentGame.Moves;
        }

        public GameInfo GetGameInfo(PgnGame game)
        {
            GameInfo info = new GameInfo();
            PgnTag pgnTag = null;
            if (game.TryGetTag(PgnTag.White, out pgnTag))
            {
                info.White = pgnTag.Value;
            }

            if (game.TryGetTag(PgnTag.Black, out pgnTag))
            {
                info.Black = pgnTag.Value;
            }

            if (game.TryGetTag(PgnTag.Event, out pgnTag))
            {
                info.Event = pgnTag.Value;
            }

            if (game.TryGetTag(PgnTag.Site, out pgnTag))
            {
                info.Site = pgnTag.Value;
            }

            if (game.TryGetTag(PgnTag.Result, out pgnTag))
            {
                info.Result = pgnTag.Value;
            }

            if (game.TryGetTag(PgnTag.Date, out pgnTag))
            {
                DateTime date;
                if(DateTime.TryParse(pgnTag.Value, out date))
                {
                    info.Date = date;
                }
            }
            return info;
        }

        public override IGame GetGame()
        {
            _currentGame = Game.CreateChessGame();
            _reader.ReadGame();
            foreach (var move in _reader.CurrentGame.Moves.Moves)
            {
                DoMove(move.Move);
                _currentGame.AddComment(_currentGame.Movelist.CurrentHalfMoveNumber, move.Comment);
            }

            _currentGame.Info = GetGameInfo(_reader.CurrentGame);

            return _currentGame;
        }
    }
}