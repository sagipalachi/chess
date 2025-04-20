using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    /// <summary>
    /// Queen chess piece
    /// </summary>
    public class Queen : Piece
    {
        public Queen(Position? pos, PieceColor color) : base(pos, color)
        {
            PieceValue = 9;
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
        {-20,-10,-10, -5, -5,-10,-10,-20,
         -10,  0,  0,  0,  0,  0,  0,-10,
         -10,  0,  5,  5,  5,  5,  0,-10,
         -5,  0,  5,  5,  5,  5,  0, -5,
          0,  0,  5,  5,  5,  5,  0, -5,
        -10,  5,  5,  5,  5,  5,  0,-10,
        -10,  0,  5,  0,  0,  0,  0,-10,
        -20,-10,-10, -5, -5,-10,-10,-20 };
    }
}
