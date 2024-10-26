using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Position? pos, PieceColor color) : base(pos, color)
        {
        }
        public override List<Position>? GetPotentialPositions()
        {
            if (Pos == null)
                return null;
            bool oneStepPossible = false;
            int dir = Color == PieceColor.White ? -1 : 1;
            List<Position>? positions = new List<Position>();
            Position p1 = new Position(Pos.Col, Pos.Row + dir);
            if (Board.GetInstance().Occupied(p1) == null)
            {
                p1.AddToList(positions);
                oneStepPossible = true;
            }
               
            if (oneStepPossible && (((Pos.Row == 6) && (Color == PieceColor.White)) || ((Pos.Row == 1) && (Color == PieceColor.Black))))
            {
                Position p2 = new Position(Pos.Col, Pos.Row + 2*dir);
                if (Board.GetInstance().Occupied(p2) == null)
                    p2.AddToList(positions);
            }
            Position p3 = new Position(Pos.Col - 1, Pos.Row +  dir);
            Position p4 = new Position(Pos.Col + 1, Pos.Row +  dir);
            if (IsEnemy(Board.GetInstance().Occupied(p3)))
            {
                p3.AddToList(positions);
            }
            if (IsEnemy(Board.GetInstance().Occupied(p4)))
            {
                p4.AddToList(positions);
            }
            return positions;
        }
    }
}
