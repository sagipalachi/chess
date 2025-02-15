using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    /// <summary>
    /// A King chess piece
    /// </summary>
    public class King : Piece
    {
        private bool isMoved = false;

        public King(Position? pos, PieceColor color) : base(pos, color)
        {
            PieceValue = 200;
        }

        /// <summary>
        /// Implement getting the possible positions
        /// Note: also support castle moves
        /// </summary>
        /// <returns></returns>
        public override List<Position>? GetPotentialPositions()
        {
            if (Pos == null)
                return null;
            int startRow = getStartRow();
           
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
            if (leftCastleAllowed())
            {
                addToPositions(new Position(Pos.Col - 3, startRow), positions);
            }
            if (rightCastleAllowed())
            {
                addToPositions(new Position(Pos.Col + 2, startRow), positions);
            }
            return positions;
        
        }

        /// <summary>
        /// Override a piece move to support castle
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="updateCheckStatus"></param>
        /// <param name="oldPositions"></param>
        /// <returns></returns>
        public override bool Move(Position targetPos, bool updateCheckStatus, out List<Position> oldPositions)
        {
            oldPositions = new List<Position> ();

            if ((targetPos.Col == 1) && (leftCastleAllowed())) // left castle
            {
                Rook? leftRook = getLeftRookForCastle();
                if (leftRook != null)
                {
                    leftRook.Move(new Position(2, getStartRow()), updateCheckStatus, out oldPositions);
                }
                oldPositions.Add(Pos);
                Pos = targetPos;
                isMoved = true;
                if (updateCheckStatus)
                {
                    Board.GetInstance().UpdateCheckStatus();
                }
                return true;
            }
            else if ((targetPos.Col == 6) && (rightCastleAllowed())) // right castle
            {
                Rook? rightRook = getRightRookForCastle();
                if (rightRook != null)
                {
                    rightRook.Move(new Position(5, getStartRow()), updateCheckStatus, out oldPositions);
                }
                oldPositions.Add(Pos);
                Pos = targetPos;
                isMoved=true;
                Board.GetInstance().UpdateCheckStatus();
                return true;
            }

            isMoved = base.Move(targetPos, updateCheckStatus, out oldPositions);
            return isMoved;
        }

        /// <summary>
        /// Check if left castle is allowed
        /// </summary>
        /// <returns></returns>
        public bool leftCastleAllowed()
        {
            if (isMoved)
                return false;
            Rook? leftRook = getLeftRookForCastle();
            if (leftRook == null)
            {
                return false;
            }
            else
            {
                if (((Rook)leftRook).isMoved)
                {
                    return false;
                }
            }
            for (int i=1; i < 4; i++)
            {
                Position p = new Position(i, getStartRow());
                Piece x = Board.GetInstance().Occupied(p);
                if (x!=null)
                    return false;
            }
            return true;
        } 

        /// <summary>
        /// Check if right castle is allowed
        /// </summary>
        /// <returns></returns>
        public bool rightCastleAllowed()
        {
            if (isMoved)
                return false;
            Rook? rightRook = getRightRookForCastle();
            if (rightRook == null)
            {
                return false;
            }
            else
            {
                if (((Rook)rightRook).isMoved)
                {
                    return false;
                }
            }
            for (int i = 5; i < 7; i++)
            {
                Position p = new Position(i, getStartRow());
                Piece x = Board.GetInstance().Occupied(p);
                if (x != null)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Get the start row to see if castle is allowed
        /// </summary>
        /// <returns></returns>
        private int getStartRow()
        {
            int startRow = 0;
            if (Color == PieceColor.White)
                startRow = 7;
            return startRow;
        }

        /// <summary>
        ///  Get the left rook for castle
        /// </summary>
        /// <returns></returns>
        private Rook? getLeftRookForCastle()
        {
            return getRookForCastle(0);
        }

        /// <summary>
        /// Get the right rook for castle
        /// </summary>
        /// <returns></returns>
        private Rook? getRightRookForCastle()
        {
            return getRookForCastle(7);
        }

        /// <summary>
        /// Helper to get the rook for castle
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private Rook? getRookForCastle(int col)
        {
            Rook rook = null;
            Piece piece = Board.GetInstance().Occupied(new Position(col, getStartRow()));
            if (piece != null && piece is Rook)
            {
                rook = (Rook)(piece);
            }
            return rook;
        }


        protected int[,] mg_King_table = new int[,]
        {
        {-65, 23,  16, -15, -56, -34,   2,  13},
        {29, -1, -20,  -7,  -8,  -4, -38, -29},
        {-9,  24,   2, -16, -20,   6,  22, -22},
        {-17, -20, -12, -27, -30, -25, -14, -36},
        {-49,  -1, -27, -39, -46, -44, -33, -51},
        {-14, -14, -22, -46, -44, -30, -15, -27},
        {1, 7,  -8, -64, -43, -16,   9,   8},
        {-15,  36,  12, -54,   8, -28,  24,  14}
        };

        protected int[,] eg_King_table = new int[,]
        {
        {-74, -35, -18, -18, -11,  15,   4, -17},
        {-12,  17,  14,  17,  17,  38,  23,  11},
        {10,  17,  23,  15,  20,  45,  44,  13},
        {-8,  22,  24,  27,  26,  33,  26,   3},
        {-18,  -4,  21,  24,  27,  23,   9, -11},
        {-19,  -3,  11,  21,  23,  16,   7,  -9},
        {-27, -11,   4,  13,  14,   4,  -5, -17},
        {-53, -34, -21, -11, -28, -14, -24, -43}
        };
    }



}
