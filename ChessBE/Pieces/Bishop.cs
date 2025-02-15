using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    /// <summary>
    /// A Bishop chess piece
    /// </summary>
    public class Bishop : Piece
    {
        public Bishop(Position? pos, PieceColor color) : base(pos, color)
        {
            PieceValue = 3;
        }
        /// <summary>
        /// Implement getting the possible positions
        /// </summary>
        /// <returns></returns>
        public override List<Position>? GetPotentialPositions()
        {
            if (Pos == null)
                return null;
            List<Position>? positions = new List<Position>();
            for (int i = 1; i < 8; i++)
            {
                Position p1 = new Position(Pos.Col + i, Pos.Row + i);
                if (!addToPositions(p1, positions))
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                Position p2 = new Position(Pos.Col + i, Pos.Row - i);
                if (!addToPositions(p2, positions))
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                Position p3 = new Position(Pos.Col - i, Pos.Row + i);
                if (!addToPositions(p3, positions))
                    break;
            }
            for (int i = 1; i < 8; i++)
            {
                Position p4 = new Position(Pos.Col - i, Pos.Row - i);
                if (!addToPositions(p4, positions))
                    break;
            }
            return positions;
        }

        protected int[,] mg_Bishop_table = new int[,]
        {
        {-29,   4, -82, -37, -25, -42,   7,  -8},
        {-26,  16, -18, -13,  30,  59,  18, -47},
        {-16,  37,  43,  40,  35,  50,  37,  -2},
        {-4,   5,  19,  50,  37,  37,   7,  -2},
        {-6,  13,  13,  26,  34,  12,  10,   4},
        {0,  15,  15,  15,  14,  27,  18,  10},
        {4,  15,  16,   0,   7,  21,  33,   1},
        {-33,  -3, -14, -21, -13, -12, -39, -21}
        };

        protected int[,] eg_Bishop_table = new int[,]
        {
        {-14, -21, -11,  -8, -7,  -9, -17, -24},
        {-8,  -4,   7, -12, -3, -13,  -4, -14},
        {2,  -8,   0,  -1, -2,   6,   0,   4},
        {-3,   9,  12,   9, 14,  10,   3,   2},
        {-6,   3,  13,  19,  7,  10,  -3,  -9},
        {-12,  -3,   8,  10, 13,   3,  -7, -15},
        {-14, -18,  -7,  -1,  4,  -9, -15, -27},
        {-23,  -9, -23,  -5, -9, -16,  -5, -17}
        };
    }
}
