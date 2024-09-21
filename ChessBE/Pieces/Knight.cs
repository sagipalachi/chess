﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    public class Knight : Piece
    {
        public Knight(Position? pos, PieceColor color) : base(pos, color)
        {
        }
        public override List<Position>? GetPotentialPositions()
        {
            if (Pos == null)
                return null;

            int dir = 1;
            int dir2 = 2;
            List<Position>? positions = new List<Position>();
            if (Color == PieceColor.White)
            {
                dir = -1;
            }
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
                if (Board.GetInstance().Occupied(p[i]) == null|| IsEnemy(Board.GetInstance().Occupied(p[i])))
                    p[i].AddToList(positions);
            }
            return positions;
        }
        public override void Move()
        {

        }
    }
}
