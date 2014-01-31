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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CChessCore.Pieces
{
    public class PieceFactory
    {
        public static IPiece CreatePiece(PieceType type, PieceColor color)
        {
            if(type == PieceType.Pawn)
            {
                return new Pawn(color);
            }

            if(type == PieceType.Knight)
            {
                return new Knight(color);
            }

            if(type == PieceType.Bishop)
            {
                return new Bishop(color);
            }

            if(type == PieceType.Rook)
            {
                return new Rook(color);
            }

            if(type == PieceType.Queen)
            {
                return new Queen(color);
            }

            if(type == PieceType.King)
            {
                return new King(color);
            }

            return new Queen(color);
        }
    }
}
