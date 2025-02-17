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
        public override int TableScore(bool middleGame)
        {
            return 0;
        }

        protected int[,] mg_Knight_table = new int[,] 
        {
        {-167, -89, -34, -49,  61, -97, -15, -107},
        {-73, -41,  72,  36,  23,  62,   7,  -17},
        {-47,  60,  37,  65,  84, 129,  73,   44},
        {-9,  17,  19,  53,  37,  69,  18,   22},
        {-13, 4,  16,  13,  28,  19,  21, -8},
        {-23,  -9,  12,  10,  19,  17,  25,  -16},
        {-29, -53, -12,  -3,  -1,  18, -14,  -19},
        {-105, -21, -58, -33, -17, -28, -19,  -23}
        };

        protected int[,] eg_Knight_table = new int[,]
        {
        {-58, -38, -13, -28, -31, -27, -63, -99},
        {-25,  -8, -25,  -2,  -9, -25, -24, -52},
        {-24, -20,  10,   9,  -1,  -9, -19, -41},
        {-17,   3,  22,  22,  22,  11,   8, -18},
        {-18,  -6,  16,  25,  16,  17,   4, -18},
        {-23,  -3,  -1,  15,  10,  -3, -20, -22},
        {-42, -20, -10,  -5,  -2, -20, -23, -44},
        {-29, -51, -23, -15, -22, -18, -50, -64}
        };
    }
}
