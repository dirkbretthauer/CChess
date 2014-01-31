#region Header
////////////////////////////////////////////////////////////////////////////// 
//    This file is part of Foobar.
//
//    Foobar is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Foobar is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
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