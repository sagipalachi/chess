using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE
{
    public class Board
    {
        public static int SIZE = 8;
        public Player player1, player2;
     
        public Board()
        {
            player1 = new Player(0);
            player2 = new Player(7);

            //Piece piece = player1.Pieces[row * Board.SIZE + col];
        }

    }
}      
