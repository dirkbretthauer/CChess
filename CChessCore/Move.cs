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
using CChessCore.Pieces;

namespace CChessCore
{
    [Flags]
    public enum MoveType
    {
        Simple = 0,
        Capture = 1,
        ShortCastle = 2,
        LongCastle = 4,
        EnPassent = 8,
        Promotion = 16,
        DoublePawn = 32,
        Check = 64,
        CheckMate = 128,
    }

    public class Move : IEquatable<Move>
    {
        public static string ShortCastleNotation = "O-O";
        public static string LongCastleNotation = "O-O-O";
        public static string CaptureNotation = "x";
        public static string CheckNotation = "+";
        public static string CheckMateNotation = "#";
        public static string PromotionNotation = "=";

        public Square From { get; private set;}
        public Square To { get; private set; }

        public int TurnNumber { get; set; }
        public int HalfMoveNumber { get; set; }
        public IPiece PieceCaptured { get; set; }
        public IPiece PieceMoved { get; set; }
        public PieceType Promotion { get; set; }
        public MoveType Type { get; set; }
        public string Position { get; set; }
        public string Notation { get { return ToString(); } }
        public string AvoidAmbiguity { get; set; }
        public string Comment { get; set; }

        public Move(Square from, Square to)
        {
            From = from;
            To = to;
            Type = MoveType.Simple;
        }

        public override string ToString()
        {
            string appendix = string.Empty;

            if(Type == MoveType.ShortCastle)
            {
                return ShortCastleNotation;
            }
            
            if (Type == MoveType.LongCastle)
            {
                return LongCastleNotation;
            }

            if ((Type & MoveType.Promotion) == MoveType.Promotion)
            {
                appendix = "=" + Promotion.PgnCode;
            }

            if ((Type & MoveType.Check) == MoveType.Check)
            {
                appendix += CheckNotation;
            }

            if ((Type & MoveType.CheckMate) == MoveType.CheckMate)
            {
                appendix += CheckMateNotation;
            }

            if(PieceCaptured != null)
            {
                if (PieceMoved is Pawn)
                {
                    return (char) (From.X + 'a') + "x" + To.ToString() + appendix;
                }

                return PieceMoved.Type.PgnCode + AvoidAmbiguity + "x" + To.ToString() + appendix;
            }

            return PieceMoved.Type.PgnCode + AvoidAmbiguity + To.ToString() + appendix;
        }

        #region Implementation of IEquatable<Move>
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Move other)
        {
            bool result = false;

            if (From.Equals(other.From) &&
                To.Equals(other.To) &&
                PieceMoved.Color == other.PieceMoved.Color &&
                PieceMoved.Type == other.PieceMoved.Type)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            return Equals(obj as Move);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 37 + From.GetHashCode();
            hash = hash * 37 + To.GetHashCode();
            return hash;
        }
        #endregion
    }
}