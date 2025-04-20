using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    /// <summary>
    /// Pawn chess piece
    /// </summary>
    public class Pawn : Piece
    {
     
        public Pawn(Position? pos, PieceColor color) : base(pos, color)
        {
            PieceValue = 1;
        }
        internal override Piece clone()
        {
            Pawn piece = new Pawn(Pos, Color);
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

        /// <summary>
        /// Override the generic move to support converstion to Queen
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="updateCheckStatus"></param>
        /// <param name="oldPositions"></param>
        /// <returns></returns>
        public override bool Move(Position targetPos, bool updateCheckStatus, out List<Position> oldPositions)
        {
            bool res = base.Move(targetPos, updateCheckStatus, out oldPositions);
            int lastRow = this.Color == PieceColor.White ? 0 : 7;
            if (this.Pos.Row == lastRow) {
                Board.GetInstance().ConvertPawnToQueen(this);
            }
            return res;
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
        private int[] table = new int[]
        { 0,  0,  0,  0,  0,  0,  0,  0,
        50, 50, 50, 50, 50, 50, 50, 50,
        10, 10, 20, 30, 30, 20, 10, 10,
        5,  5, 10, 25, 25, 10,  5,  5,
        0,  0,  0, 20, 20,  0,  0,  0,
        5, -5,-10,  0,  0,-10, -5,  5,
        5, 10, 10,-20,-20, 10, 10,  5,
        0,  0,  0,  0,  0,  0,  0,  0};
    }
}
