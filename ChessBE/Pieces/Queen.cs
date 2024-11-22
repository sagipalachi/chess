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
    }
}
