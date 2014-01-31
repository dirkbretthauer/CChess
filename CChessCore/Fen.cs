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
    public class Fen
    {
        public static string GetFen(IGame game, bool onlyPosition)
        {
            string output = String.Empty;
            string spacer = "";
            byte blankSquares = 0;

            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    IPiece piece = game.Board[new Square(x, y)];
                    if (piece != null)
                    {
                        if (blankSquares > 0)
                        {
                            output += blankSquares.ToString();
                            blankSquares = 0;
                        }

                        output += piece.FenAbbreviation;
                    }
                    else
                    {
                        blankSquares++;
                    }

                    if (x == 7)
                    {
                        if (blankSquares > 0)
                        {
                            output += blankSquares.ToString();
                            output += "/";
                            blankSquares = 0;
                        }
                        else
                        {
                            if (y != 0)
                            {
                                output += "/";
                            }
                        }
                    }
                }
            }

            if (game.Status.Turn == PieceColor.White)
            {
                output += " b ";
            }
            else
            {
                output += " w ";
            }

            bool canCastle = false;
            if (game.Status.CanWhiteCastleShort)
            {
                canCastle = true;
                output += "K";
            }
            if (game.Status.CanWhiteCastleLong)
            {
                canCastle = true;
                output += "Q";
            }

            if (game.Status.CanBlackCastleShort)
            {
                canCastle = true;
                output += "k";
            }

            if (game.Status.CanBlackCastleLong)
            {
                canCastle = true;
                output += "q";
            }

            if(!canCastle)
            {
                output += " - ";
            }
            else
            {
                output += " ";
            }

            if (output.EndsWith("/"))
            {
                output.TrimEnd('/');
            }


            if (game.Status.EnPassentPosition != null)
            {
                output += game.Status.EnPassentPosition.ToString() + " ";
            }
            else
            {
                output += "- ";
            }

            if(onlyPosition)
            {
                return output.Trim();
            }

            output += game.Status.MoveFifty + " ";
            output += game.Movelist.Count + 1;
            
            return output.Trim();
        }

        public static IGame GetBoard(string fen)
        {
            IGame game = Game.CreateChessGame();
            game.Board.ClearBoard();

            string[] fenGroups = fen.Split(' ');

            string[] rows = fenGroups[0].Split('/');
            int currentY = 7;
            foreach (var row in rows)
            {
                int currentX = 0;
                foreach (var c in row)
                {
                    int emptyfields;
                    if (Int32.TryParse(c.ToString(), out emptyfields))
                    {
                        currentX += emptyfields;
                        continue;
                    }
                    else
                    {
                        game.Board[new Square(currentX, currentY)] = GetPiece(c);
                        currentX += 1;
                    }
                }
                currentY--;
            }

            game.Status.Turn = fenGroups[1].Equals("w") ? PieceColor.White : PieceColor.Black;
            game.Status.CanBlackCastleLong = false;
            game.Status.CanBlackCastleShort = false;
            game.Status.CanWhiteCastleLong = false;
            game.Status.CanWhiteCastleShort = false;
            if(fenGroups[2].Contains("K"))
            {
                game.Status.CanWhiteCastleShort = true;
            }
            if (fenGroups[2].Contains("Q"))
            {
                game.Status.CanWhiteCastleLong = true;
            }
            if (fenGroups[2].Contains("k"))
            {
                game.Status.CanBlackCastleShort = true;
            }
            if (fenGroups[2].Contains("q"))
            {
                game.Status.CanBlackCastleLong = true;
            }

            if(!fenGroups[3].Equals("-"))
            {
                game.Status.EnPassentPosition = fenGroups[3];
            }

            if(fenGroups.Length > 4)
            {
                game.Status.MoveFifty = Int32.Parse(fenGroups[4]);    
            }

            return game;
        }

        private static IPiece GetPiece(char c)
        {
            IPiece result = null;
            if (c == 'P')
            {
                result = new Pawn(PieceColor.White);
            }
            else if (c == 'N')
            {
                result = new Knight(PieceColor.White);
            }
            else if (c == 'B')
            {
                result = new Bishop(PieceColor.White);
            }
            else if (c == 'R')
            {
                result = new Rook(PieceColor.White);
            }
            else if (c == 'Q')
            {
                result = new Queen(PieceColor.White);
            }
            else if (c == 'K')
            {
                result = new King(PieceColor.White);
            }
            else if (c == 'p')
            {
                result = new Pawn(PieceColor.Black);
            }
            else if (c == 'n')
            {
                result = new Knight(PieceColor.Black);
            }
            else if (c == 'b')
            {
                result = new Bishop(PieceColor.Black);
            }
            else if (c == 'r')
            {
                result = new Rook(PieceColor.Black);
            }
            else if (c == 'q')
            {
                result = new Queen(PieceColor.Black);
            }
            else if (c == 'k')
            {
                result = new King(PieceColor.Black);
            }

            return result;
        }
    }
}
