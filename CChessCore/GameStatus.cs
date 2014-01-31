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

namespace CChessCore
{
    public enum Result
    {
        None,
        WhiteWon,
        BlackWon,
        Draw
    }

    public class GameStatus
    {
        public Result Result { get; set; }

        public PieceColor Turn { get; set; }

        public bool CanWhiteCastleLong { get; set; }

        public bool CanWhiteCastleShort { get; set; }

        public bool CanBlackCastleLong { get; set; }

        public bool CanBlackCastleShort { get; set; }

        public Square EnPassentPosition { get; set; }

        public int MoveFifty { get; set; }

        public GameStatus()
        {
            Turn = PieceColor.White;
            Result = Result.None;

            CanWhiteCastleLong = true;
            CanWhiteCastleShort = true;
            CanBlackCastleLong = true;
            CanBlackCastleShort = true;
        }

        public static string GetResult(Result result)
        {
            switch (result)
            {
                case Result.None:
                    return "*";
                case Result.WhiteWon:
                    return "1-0";
                case Result.BlackWon:
                    return "0-1";
                case Result.Draw:
                    return "1/2-1/2";

                default:
                    return "*";
            }
        }
    }
}
