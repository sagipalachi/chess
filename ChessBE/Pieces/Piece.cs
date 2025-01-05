
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
    /// <summary>
    /// An abstract chess piece - defines the interface of all the pieces
    /// and the common behavior of the chess pieces
    /// </summary>
    public abstract class Piece
    {
        public Position? Pos { get; set; }
        public PieceColor Color = PieceColor.Black;
        public int PieceValue;
        protected Piece? lastCapturedEnemyPiece;
        /// <summary>
        /// Constrcutor - position and color
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="color"></param>
        public Piece(Position? pos, PieceColor color)
        {
            Pos = pos;
            Color = color;
            lastCapturedEnemyPiece = null;
        }

        /// <summary>
        /// Returns true if the piece belongs to the other player (by color), otherwise false
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsEnemy(Piece? p)
        {
            return p != null && p.Color != Color;
        }

        /// <summary>
        /// Returns all potential positions of the piece - implemented by the concrete piece classes
        /// </summary>
        /// <returns></returns>
        public virtual List<Position>? GetPotentialPositions()
        { 
            return null; 
        }

        /// <summary>
        /// Perform a move - some concrete pieces override override this method, for example, pawn or king
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="updateCheckStatus"></param>
        /// <param name="oldPositions"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Undo a move (used by board evaluation and potentially for undo in the future)
        /// </summary>
        /// <param name="src"></param>
        internal void UndoMove(Position? src)
        {
            Pos = src;
            Board.GetInstance().RestorePiece(lastCapturedEnemyPiece);
        }

        /// <summary>
        /// A helper method to add to the list of possible positions
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="posList"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all the possible moves the piece can do in the current board state
        /// </summary>
        /// <returns></returns>
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

    /// <summary>
    /// Helper class to represent a position of a piece
    /// </summary>
    public class Position
    {
        public int Row { get; set; }
        public int Col { get; set; }

        /// <summary>
        /// Initialize the position (constructor)
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        public Position(int col, int row)
        {
            Row = row;
            Col = col;
        }

        /// <summary>
        /// True if the position is in the limits of the board, otherwise false
        /// </summary>
        /// <returns></returns>
        private bool inBounds()
        {
            return (Row >= 0 && Col >= 0 && Row <= 7 && Col <= 7);
        }

        /// <summary>
        /// True if the input position is the same as the position
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool isEqual(Position p)
        {
            return (p.Row == Row && p.Col == Col);
        }

        /// <summary>
        /// Adds the position to a list if it is a valid position (on the board)
        /// </summary>
        /// <param name="posList"></param>
        public void AddToList(List<Position> posList)
        {
            if (inBounds())
            {
                posList.Add(this);
            }
        }
    }

    /// <summary>
    /// Enumarted type for piece color (black/white)
    /// </summary>
    public enum PieceColor
    {
        White,
        Black,
    }
    

  
}
