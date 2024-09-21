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
            int dir = 1;
            
            if (Pos != null)
            {
                List<Position>? positions = new List<Position>();
                if (Color == PieceColor.White)
                {
                    dir = -1; 
                }
                Position p1 = new Position(Pos.Row + dir, Pos.Col);
                if (Board.GetInstance().Occupied(p1) != null)
                    positions.Add(p1);
                if (((Pos.Col == 6) && (Color == PieceColor.White)) || ((Pos.Col == 1) && (Color == PieceColor.Black)))
                {
                    Position p2 = new Position(Pos.Row + 2*dir, Pos.Col);
                    if (Board.GetInstance().Occupied(p2) != null)
                        positions.Add(p2);
                }
                Position p3 = new Position(Pos.Row +  dir, Pos.Col-1);
                Position p4 = new Position(Pos.Row +  dir, Pos.Col+1);
                if (IsEnemy(Board.GetInstance().Occupied(p3)))
                {
                    positions.Add(p3);
                }
                if (IsEnemy(Board.GetInstance().Occupied(p4)))
                {
                    positions.Add(p4);
                }
            }
            return null; 
        }
        public override void Move()
        {

        }
    }
}
