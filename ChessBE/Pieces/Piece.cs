
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    /// <summary>
    /// An abstract chess piece - defines the interface of all the pieces
    /// and the common behaviour  of the chess pieces
    /// </summary>
    public abstract class Piece
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected static int[,] flipTable(int[,] orgTable)
        {
            int rows = orgTable.GetLength(0);
            int cols = orgTable.GetLength(1);

            int[,] flippedTable = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    flippedTable[i, j] = orgTable[rows - 1 - i, cols - 1 - j];
                }
            }
            return flippedTable;
        }
        protected static void printTable(int[,] table)
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                String row = "";
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    row += (table[i, j] + ",");
                }
                Logger.Info(row);
            }
        }



        public Position? Pos { get; set; }
        public PieceColor Color = PieceColor.Black;
        public int PieceValue;
        private Stack<Piece> lastCapturedEnemyPieces = new Stack<Piece>();
        /// <summary>
        /// Constrcutor - position and color
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="color"></param>
        public Piece(Position? pos, PieceColor color)
        {
            Pos = pos;
            Color = color;
        }

        ~Piece()
        {
            Logger.Info("Piece destroyed");
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
        /// Perform a move - some concrete pieces override this method, for example, pawn or king
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
                if (otherPiece != null && IsEnemy(otherPiece))
                {
                    if (Board.GetInstance().RemovePiece(otherPiece))
                        lastCapturedEnemyPieces.Push(otherPiece);
                    else
                        Logger.Error("Failed to remove piece " + otherPiece);
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
        internal virtual void UndoMove(Position? src)
        {
            Pos = src;
            if (lastCapturedEnemyPieces.Count>0)
            {
                Board.GetInstance().RestorePiece(lastCapturedEnemyPieces.Pop());
            }

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


        /// <summary>
        /// True if the input piece is the same as the piece
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool isEqual(Piece p)
        {
            return (p.GetType() == this.GetType() && p.Color == Color && p.Pos.isEqual(Pos));
        }

        /// <summary>
        /// For logging and debugging purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetType()+" col is "+ Pos.Col+" row is "+ Pos.Row + " " + PieceValue + " " + Color;
        }

     
        public void ClearLastCapturedEnemyPieces()
        {
            lastCapturedEnemyPieces.Clear();
        }

        /// <summary>
        /// For cloning the pieces - must be overridden by every concrete piece 
        /// (calling non default constructor)
        /// </summary>
        /// <returns></returns>
        internal abstract Piece clone();
        
        /// <summary>
        /// For calculating positional score - must be overridden by every concrete piece
        /// </summary>
        /// <param name="ind"></param>
        /// <returns></returns>
        public abstract int GetTableScore(int ind);
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
