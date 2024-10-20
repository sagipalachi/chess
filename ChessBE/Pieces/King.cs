using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    public class King : Piece
    {
        public King(Position? pos, PieceColor color) : base(pos, color)
        {
        }
      
        public override List<Position>? GetPotentialPositions()
        {
            if (Pos == null)
                return null;
                 
            List<Position>? positions = new List<Position>();
            Position[] p = new Position[8];
            p[0] = new Position(Pos.Col + 1, Pos.Row );
            p[1] = new Position(Pos.Col - 1, Pos.Row );
            p[2] = new Position(Pos.Col + 1, Pos.Row - 1);
            p[3] = new Position(Pos.Col + 1, Pos.Row + 1);
            p[4] = new Position(Pos.Col - 1, Pos.Row - 1);
            p[5] = new Position(Pos.Col - 1, Pos.Row + 1);
            p[6] = new Position(Pos.Col , Pos.Row - 1);
            p[7] = new Position(Pos.Col , Pos.Row + 1);
            for (int i = 0; i < 8; i++)
            {
                addToPositions(p[i], positions);
            }
            return positions;
        }
    }


}
