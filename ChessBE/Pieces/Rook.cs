using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    public class Rook : Piece
    {
        public Rook(Position? pos, PieceColor color) : base(pos, color)
        {
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
            for (int i = Pos.Col+1; i < 8; i++)
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

        private bool addToPositions(Position pos, List<Position> posList)
        {
            Piece otherPiece = Board.GetInstance().Occupied(pos);
            if (otherPiece == null)
            {
                pos.AddToList(posList);
                return true;
            }
            else if (IsEnemy(otherPiece))
            {
                pos.AddToList(posList);
                return false;
            }
            return false;
        }
    }
}


