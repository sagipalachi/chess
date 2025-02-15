using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    public class Queen : Piece
    {
        public Queen(Position? pos, PieceColor color) : base(pos, color)
        {
            PieceValue = 9;
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

        protected int[,] mg_Queen_table = new int[,]
        {
        {-28, 0, 29,  12, 59, 44, 43, 45},
        {-24, -39, -5, 1, -16, 57, 28, 54},
        {-13, -17, 7, 8, 29, 56, 47, 57},
        {-27, -27, -16, -16, -1, 17, -2, 1},
        {-9, -26, -9, -10, -2, -4, 3, -3},
        {-14, 2, -11, -2, -5, 2, 14, 5},
        {-35, -8, 11, 2, 8, 15, -3, 1},
        {-1, -18, -9, 10, -15, -25, -31, -50}
        };

        protected int[,] eg_Queen_table = new int[,]
        {
        {-9, 22, 22, 27, 27, 19, 10, 20},
        {-17, 20, 32, 41, 58, 25, 30, 0},
        {-20, 6, 9, 49, 47, 35, 19, 9},
        {3, 22, 24, 45, 57, 40, 57, 36},
        {-18, 28, 19, 47, 31, 34, 39, 23},
        {-16, -27,  15, 6, 9,  17,  10, 5},
        {-22, -23, -30, -16, -16, -23, -36, -32},
        {-33, -28, -22, -43,  -5, -32, -20, -41}
        };
    }
}
