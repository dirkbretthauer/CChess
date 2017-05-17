#region Header
////////////////////////////////////////////////////////////////////////////// 
//The MIT License (MIT)

//Copyright (c) 2013 Dirk Bretthauer

//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
            
            foreach (var move in game.Moves)
            {
                DoMove(move);
                _currentGame.AddComment(_currentGame.Movelist.CurrentHalfMoveNumber, move.Comment);
            }

            _currentGame.Info = GetGameInfo(game);

            return _currentGame;
        }

        public IList<PgnMove> GetMoves()
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
            foreach (var move in _reader.CurrentGame.Moves)
            {
                var convertedMove = DoMove(move);
                if(convertedMove != null)
                {
                    convertedMove.Comment = move.Comment;
                }
            }

            _currentGame.Info = GetGameInfo(_reader.CurrentGame);

            return _currentGame;
        }
    }
}