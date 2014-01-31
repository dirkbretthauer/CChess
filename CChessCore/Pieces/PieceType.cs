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

namespace CChessCore
{
    public class PieceType
    {
        private string _pgnCode;

        public string PgnCode { get { return _pgnCode; } }

        public static PieceType Pawn = new PieceType("");
        public static PieceType Knight = new PieceType("N");
        public static PieceType Bishop = new PieceType("B");
        public static PieceType Rook = new PieceType("R");
        public static PieceType Queen = new PieceType("Q");
        public static PieceType King = new PieceType("K");

        private PieceType(string pgnCode)
        {
            _pgnCode = pgnCode;
        }

        public static PieceType GetPieceType(string pgnCode)
        {
            pgnCode.Trim();
            switch (pgnCode)
            {
                case "":
                    return PieceType.Pawn;
                case "B":
                    return PieceType.Bishop;
                case "N":
                    return PieceType.Knight;
                case "R":
                    return PieceType.Rook;
                case "Q":
                    return PieceType.Queen;
                case "K":
                    return PieceType.King;
                default:
                    return null;
            }
        }
    }
}