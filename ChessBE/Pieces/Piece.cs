
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    public abstract class Piece
    {
        public Position? Pos { get; set; }
        public PieceColor Color = PieceColor.Black;
        public int PieceValue;
        protected Piece? lastCapturedEnemyPiece;
        public Piece(Position? pos, PieceColor color)
        {
            Pos = pos;
            Color = color;
            lastCapturedEnemyPiece = null;
        }

        public bool IsEnemy(Piece? p)
        {
            return p != null && p.Color != Color;
        }

        public virtual List<Position>? GetPotentialPositions()
        { 
            return null; 
        }

        public virtual bool Move(Position targetPos, bool updateCheckStatus, out List<Position> oldPositions)
        {
            oldPositions = new List<Position>();
            List<Position>? positions = GetPotentialPositions();
            if (positions != null && positions.Any(p => p.isEqual(targetPos)))
            {
                Piece otherPiece = Board.GetInstance().Occupied(targetPos);
                lastCapturedEnemyPiece = null;
                if (otherPiece != null && IsEnemy(otherPiece))
                {
                    Board.GetInstance().RemovePiece(otherPiece);
                    lastCapturedEnemyPiece = otherPiece;
                }
                oldPositions.Add(Pos);
                Pos = targetPos;
                if (updateCheckStatus)
                {
                    Board.GetInstance().UpdateCheckStatus();
                }
                return true;
            }
            return false;
        }

        internal void UndoMove(Position? src)
        {
            Pos = src;
            Board.GetInstance().RestorePiece(lastCapturedEnemyPiece);
        }

        protected bool addToPositions(Position pos, List<Position> posList)
        {
            Piece otherPiece = Board.GetInstance().Occupied(pos);
            CheckStatus checkStatus = CheckStatus.None;
            if (otherPiece == null)
            {
                pos.AddToList(posList);
                return true;
            }
            else if (IsEnemy(otherPiece))
            {
                if (otherPiece is King)
                {
                    checkStatus = (otherPiece.Color==PieceColor.White) ? CheckStatus.White : CheckStatus.Black;
                    Board.GetInstance().BoardCheckStatus = checkStatus;
                }
                pos.AddToList(posList);
                return false;
            }
            return false;
        }

        public List<Move> GetPossibleMoves()
        {
            List<Move> moves = new List<Move>();
            List<Position>? potPositions = GetPotentialPositions();
            if (potPositions != null)
            {
                foreach(Position dest in potPositions) {
                    moves.Add(new ChessBE.Move(this, Pos, dest));                    
                }
            }
            return moves;
        }

    }

    public class Position
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Position(int col, int row)
        {
            Row = row;
            Col = col;
        }

        private bool inBounds()
        {
            return (Row >= 0 && Col >= 0 && Row <= 7 && Col <= 7);
        }

        public bool isEqual(Position p)
        {
            return (p.Row == Row && p.Col == Col);
        }

        public void AddToList(List<Position> posList)
        {
            if (inBounds())
            {
                posList.Add(this);
            }
        }
    }

    public enum PieceColor
    {
        White,
        Black,
    }
    

  
}
