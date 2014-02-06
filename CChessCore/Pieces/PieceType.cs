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