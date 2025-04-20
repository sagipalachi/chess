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

        internal override Piece clone()
        {
            King piece = new King(Pos, Color);
            return piece;
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

        /// <summary>
        /// For calculating positional score
        /// The King is the only piece taking into accout
        /// the game phase: 
        /// egTable - end game table
        /// mgTable - middle game table
        /// </summary>
        /// <param name="ind"></param>
        /// <returns></returns>
        public override int GetTableScore(int ind)
        {
            if(Board.GetInstance().isEndStage())
            return egTable[ind];
            return mgTable[ind];
        }

        private int[] mgTable = new int[]
        {
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -20,-30,-30,-40,-40,-30,-30,-20,
        -10,-20,-20,-20,-20,-20,-20,-10,
         20, 20,  0,  0,  0,  0, 20, 20,
         20, 30, 10,  0,  0, 10, 30, 20 
        };

        private int[] egTable = new int[]
       {
       -50,-40,-30,-20,-20,-30,-40,-50,
       -30,-20,-10,  0,  0,-10,-20,-30,
       -30,-10, 20, 30, 30, 20,-10,-30,
       -30,-10, 30, 40, 40, 30,-10,-30,
       -30,-10, 30, 40, 40, 30,-10,-30,
       -30,-10, 20, 30, 30, 20,-10,-30,
       -30,-30,  0,  0,  0,  0,-30,-30,
       -50,-30,-30,-30,-30,-30,-30,-50 
       };
    }



}
