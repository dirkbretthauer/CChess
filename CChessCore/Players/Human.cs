using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CChessCore.Players
{
    public class Human : IPlayer
    {
        public PieceColor Color { get; private set; }

        public Human(PieceColor color)
        {
            Color = color;
        }

        public void YourTurn(IGame game)
        {
            
        }
    }
}
