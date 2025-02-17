using NLog;
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
            printTable(mg_pawn_table);
            int [,] flippedTable = flipTable(mg_pawn_table);
            printTable(flippedTable);
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
        public override int TableScore(bool middleGame)
        {
            return 0;
        }

        protected int[,] mg_pawn_table = new int[,]
        {
        {0,    0,  0, 0, 0, 0, 0, 0 },
        {98, 134, 61, 95, 68, 126, 34, -11 },
        {-6,   7, 26, 31, 65, 56, 25, -20 },
        {-14, 13, 6, 21, 23, 12, 17, -23 },
        {-27, -2, -5, 12, 17, 6, 10, -25},
        {-26, -4, -4, -10, 3, 3, 33, -12},
        {-35, -1, -20, -23, -15, 24, 38, -22},
        {0, 0, 0, 0, 0, 0, 0, 0}
        };

        protected int[,] eg_pawn_table = new int[,]
        {
        {0, 0, 0, 0, 0, 0, 0, 0 },
        {178, 173, 158, 134, 147, 132, 165, 187},
        {94, 100,  85,  67,  56,  53,  82,  84},
        {32,  24,  13,   5,  -2,   4,  17,  17},
        {13,   9,  -3,  -7,  -7,  -8,   3,  -1},
        {4,   7,  -6,   1,   0,  -5,  -1,  -8},
        {13,   8,   8,  10,  13,   0,   2,  -7},
        {0, 0, 0, 0, 0, 0, 0, 0 }
        };
    }
}
