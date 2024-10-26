using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    public class King : Piece
    {
        private bool isMoved = false;

        public King(Position? pos, PieceColor color) : base(pos, color)
        {
        }
      
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
        public override bool Move(Position targetPos, out List<Position> oldPositions)
        {
            oldPositions = new List<Position> ();

            if ((targetPos.Col == 1) && (leftCastleAllowed())) // left castle
            {
                Rook? leftRook = getLeftRookForCastle();
                if (leftRook != null)
                {
                    leftRook.Move(new Position(2, getStartRow()), out oldPositions);
                }
                oldPositions.Add(Pos);
                Pos = targetPos;
                isMoved = true;
                Board.GetInstance().UpdateCheckStatus();
                return true;
            }
            else if ((targetPos.Col == 6) && (rightCastleAllowed())) // right castle
            {
                Rook? rightRook = getRightRookForCastle();
                if (rightRook != null)
                {
                    rightRook.Move(new Position(5, getStartRow()), out oldPositions);
                }
                oldPositions.Add(Pos);
                Pos = targetPos;
                isMoved=true;
                Board.GetInstance().UpdateCheckStatus();
                return true;
            }

            isMoved = base.Move(targetPos, out oldPositions);
            return isMoved;
        }
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
        private int getStartRow()
        {
            int startRow = 0;
            if (Color == PieceColor.White)
                startRow = 7;
            return startRow;
        }

        private Rook? getLeftRookForCastle()
        {
            return getRookForCastle(0);
        }

        private Rook? getRightRookForCastle()
        {
            return getRookForCastle(7);
        }

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

    }



}
