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

using System;
using System.Collections.Generic;

namespace CChessCore
{
    public interface IPiece
    {
        /// <summary>
        /// Gets the color.
        /// </summary>
        PieceColor Color { get;  }

        /// <summary>
        /// Gets the type.
        /// </summary>
        PieceType Type { get; }

        /// <summary>
        /// Abbreviation used in FEN notation.
        /// </summary>
        String FenAbbreviation { get; }

        /// <summary>
        /// Determines whether this instance can execute the
        /// specified move on the given chess board.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="board">The chess board.</param>
        /// <param name="move">The move to do.</param>
        /// <returns>
        ///   <c>true</c> if this instance can execute the specified move; otherwise, <c>false</c>.
        /// </returns>
        bool CanMove(IGame game, IGameBoard board, Move move);

        /// <summary>
        /// Gets the valid moves for this piece sitting on <paramref name="square"/>.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="board">The board.</param>
        /// <param name="square">The square.</param>
        /// <returns></returns>
        IEnumerable<Move> GetValidMoves(IGame game, IGameBoard board, Square square);
    }
}