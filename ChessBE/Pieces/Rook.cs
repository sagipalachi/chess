using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    public class Rook : Piece
    {
        public bool isMoved { get; set; } = false;
        public Rook(Position? pos, PieceColor color) : base(pos, color)
        {
            PieceValue = 5;
        }

        internal override Piece clone()
        {
            Piece piece = new Queen(Pos, Color);
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
            List<Position>? positions = new List<Position>();
            for (int i = Pos.Row + 1; i < 8; i++)
            {
                Position pRow = new Position(Pos.Col, i);
                if (!addToPositions(pRow, positions))
                    break;
            }
            for (int i = Pos.Row - 1; i >= 0; i--)
            {
                Position pRow = new Position(Pos.Col, i);
                if (!addToPositions(pRow, positions))
                    break;
            }
            for (int i = Pos.Col + 1; i < 8; i++)
            {
                Position pCol = new Position(i, Pos.Row);
                if (!addToPositions(pCol, positions))
                    break;
            }
            for (int i = Pos.Col - 1; i >= 0; i--)
            {
                Position pCol = new Position(i, Pos.Row);
                if (!addToPositions(pCol, positions))
                    break;
            }
            return positions;
        }

        public override int TableScore(bool middleGame)
        {
            return 0;
        }

        protected int[,] mg_table = new int[,]
        {
        {32,  42,  32,  51, 63,  9,  31,  43 },
        {27,  32,  58,  62, 80, 67,  26,  44},
        {-5,  19,  26,  36, 17, 45,  61,  16},
        {-24, -11,   7,  26, 24, 35,  -8, -20},
        {-36, -26, -12,  -1,  9, -7,   6, -23},
        {-45, -25, -16, -17,  3,  0,  -5, -33},
        {-44, -16, -20,  -9, -1, 11,  -6, -71},
        {-19, -13,   1,  17, 16,  7, -37, -26}
        };

        protected int[,] eg_table = new int[,]
        {
        {13, 10, 18, 15, 12,  12,   8,   5},
        {11, 13, 13, 11, -3,   3,   8,   3},
        {7,  7,  7,  5,  4,  -3,  -5,  -3},
        {4,  3, 13,  1,  2,   1,  -1,   2},
        {3,  5,  8,  4, -5,  -6,  -8, -11},
        {-4,  0, -5, -1, -7, -12,  -8, -16},
        {-6, -6,  0,  2, -9,  -9, -11,  -3},
        {-9,  2,  3, -1, -5, -13,   4, -20}
        };

    }
}


