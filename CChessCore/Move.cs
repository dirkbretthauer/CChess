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

            if(!string.IsNullOrWhiteSpace(Comment))
            {
                appendix += " " + Comment;
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