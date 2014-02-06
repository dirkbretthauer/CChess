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
