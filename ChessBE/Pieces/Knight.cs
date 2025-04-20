using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    /// <summary>
    /// Knight chess piece
    /// </summary>
    public class Knight : Piece
    {
        public Knight(Position? pos, PieceColor color) : base(pos, color)
        {
            PieceValue = 3;
        }

        internal override Piece clone()
        {
            Knight piece = new Knight(Pos, Color);
            return piece;
        }

        /// <summary>
        /// Implement getting the possible positions
        /// </summary>
        /// <returns></returns>
        public override List<Position>? GetPotentialPositions()
        {
            if (Pos == null)
                return null;

            int dir = 1;
            int dir2 = 2;
            List<Position>? positions = new List<Position>();
            Position[] p = new Position[8];
            p[0] = new Position(Pos.Col+dir, Pos.Row + dir2);
            p[1] = new Position(Pos.Col + dir * -1, Pos.Row + dir2);
            p[2] = new Position(Pos.Col + dir, Pos.Row + dir2 * -1);
            p[3] = new Position(Pos.Col + dir * -1, Pos.Row + dir2 * -1);
            p[4] = new Position(Pos.Col + dir2, Pos.Row + dir);
            p[5] = new Position(Pos.Col + dir2 * -1, Pos.Row + dir);
            p[6] = new Position(Pos.Col + dir2, Pos.Row + dir * -1);
            p[7] = new Position(Pos.Col + dir2 * -1, Pos.Row + dir * -1);
            for(int i = 0; i < 8; i++)
            {
                addToPositions(p[i], positions);
            }
            return positions;
        }

        /// <summary>
        /// For calculating positional score
        /// </summary>
        /// <param name="ind"></param>
        /// <returns></returns>
        public override int GetTableScore(int ind)
        {
            return table[ind];
        }

        public int[] table = new int[] 
        {-50,-40,-30,-30,-30,-30,-40,-50,
         -40,-20,  0,  0,  0,  0,-20,-40,
         -30,  0, 10, 15, 15, 10,  0,-30,
         -30,  5, 15, 20, 20, 15,  5,-30,
         -30,  0, 15, 20, 20, 15,  0,-30,
         -30,  5, 10, 15, 15, 10,  5,-30,
         -40,-20,  0,  5,  5,  0,-20,-40,
         -50,-40,-30,-30,-30,-30,-40,-50, };
    }
}
