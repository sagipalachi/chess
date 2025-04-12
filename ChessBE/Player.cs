using ChessBE.Pieces;
using Microsoft.VisualBasic.FileIO;
using NLog;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;

namespace ChessBE
{
    /// <summary>
    /// Class representing a chess player, the player "owns" pieces and performs moves
    /// </summary>
    public class Player
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        BoardEvaluation boardEvaluation = null;
        public List<Piece> Pieces = new();

        public PieceColor Color;

        /// <summary>
        /// Constructor - initialize the pieces of the player 
        /// </summary>
        /// <param name="startRow"></param>
        public Player(int startRow)
        {
            Color = startRow == 0 ? PieceColor.Black : PieceColor.White;
            Pieces.Add(new Rook(new Position(0, startRow), Color));
            Pieces.Add(new Knight(new Position(1, startRow), Color));
            Pieces.Add(new Bishop(new Position(2, startRow), Color));
            Pieces.Add(new Queen(new Position(3, startRow), Color));
            Pieces.Add(new King(new Position(4, startRow), Color));
            Pieces.Add(new Bishop(new Position(5, startRow), Color));
            Pieces.Add(new Knight(new Position(6, startRow), Color));
            Pieces.Add(new Rook(new Position(7, startRow), Color));

            int row = startRow == 0 ? startRow + 1 : startRow - 1;
            for (int i = 0; i < Board.SIZE; i++)
                Pieces.Add(new Pawn(new Position(i, row), Color));

        }

        /// <summary>
        /// Return the number of pieces of a specific type (e.g., Pawm, Bishop, King)
        /// </summary>
        /// <param name="pieceType"></param>
        /// <returns></returns>
        public int GetPieceCount(Type pieceType)
        {
            int counter = 0;
            foreach(Piece p in Pieces)
            {
                if (p.GetType() == pieceType)
                    counter++;
            }
            return counter;
        }

        /// <summary>
        /// Returns true if a position is occupied by a piece, otherwise false
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Piece? PosOccupied(Position pos)
        {
            foreach (var piece in Pieces)
            {
                Position? pp = piece.Pos;
                if (pp != null && pp.isEqual(pos))
                {
                    return piece;
                }
            }
            return null;
        }

        /// <summary>
        /// Remove a piece due to piece capture
        /// </summary>
        /// <param name="piece"></param>
        public bool RemovePiece(Piece piece)
        {
            bool res = Pieces.Remove(piece);
            if (piece is Pawn && piece.Color == PieceColor.White && piece.Pos.Row == 2) {
                Logger.Info("REMOVING " + piece.ToString());
            }

            if (piece is King)
            {
                Board.GetInstance().Checkmate(piece.Color);
            }
            return res;
        }

        /// <summary>
        /// Restores a captured piece
        /// </summary>
        /// <param name="piece"></param>
        internal void RestorePiece(Piece piece)
        {
            if (Pieces.Any(p=>p.isEqual(piece))) {
                Logger.Error("Restored called for an existing piece " + piece.ToString());
                return;
            }
            Logger.Info("piece added " + piece.ToString());
            Pieces.Add(piece);
        }

        /// <summary>
        /// Returns the position of the king, null if king is not found
        /// </summary>
        /// <returns></returns>
        public Position? GetKingPos()
        {
            foreach (var piece in Pieces)
            {
                if (piece is King)
                {
                    return piece.Pos;
                }
                
            }
            return null;
        }

        /// <summary>
        /// Get the possible moves the player can do
        /// </summary>
        /// <returns></returns>
        internal List<Move> GetPossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();
            foreach (var piece in Pieces)
            {
                possibleMoves.AddRange(piece.GetPossibleMoves());
            }
            return possibleMoves;
        }

        /// <summary>
        /// Sets the player in auto mode - meaning the computer will play ("AI Play")
        /// </summary>
        /// <param name="auto"></param>
        public void SetAutoMode(bool auto) 
        {
            if (auto)
                boardEvaluation = new BoardEvaluation();
            else
                boardEvaluation = null;
        }

        /// <summary>
        /// Returns true if player is played by the computer, otherwise false
        /// </summary>
        /// <returns></returns>
        public bool IsAutoMode()
        {
            if (boardEvaluation != null)
                return true;
            return false;
        }

        /// <summary>
        /// Perform an auto move (using the board evaluation)
        /// </summary>
        /// <param name="oldPositions"></param>
        internal void DoAutoMove(out List<Position> oldPositions)
        {
            //Board.GetInstance().backup();
            Move bestMove = boardEvaluation.BestMove(Board.GetInstance());
            //Board.GetInstance().restore();
            Piece? newPiece = Board.GetInstance().GetPieceByPos(bestMove.piece.Pos);
            if (newPiece == null)
            {
                Logger.Error("Unable to find piece for Best Move");
            }
            newPiece.Move(bestMove.dest, true, out oldPositions);
            oldPositions.Add(bestMove.piece.Pos); // add the current position to oldPosition to enforce refresh in the FE
            List<Position> dummy;
            Board.GetInstance().passTurn(out dummy);
        }
        public override string ToString()
        {
            int counter = 0;
            string Tss = "";
            foreach (var piece in Pieces)
            {
                Tss = Tss+" "+piece.ToString();
                counter++;
            }
            return counter+" "+Tss;
        }

        public bool QueenReachedLastRow()
        {
            int lastRow = this.Color == PieceColor.White ? 0 : 7;
            foreach (Piece piece in Pieces)
            {
                if ((piece is Queen) && (piece.Pos.Row == lastRow))
                    return true;
            }
            return false;
        }

        internal void ConvertPawnToQueen(Pawn pawn)
        {
            /*
            Queen queen = new Queen(pawn.Pos, pawn.Color);
            RemovePiece(pawn);
            Pieces.Add(queen);
            */
        }

        internal Player? clone()
        {
            int startRow = 0;
            if (Color == PieceColor.White)
                startRow = 7;
            Player res = new Player(startRow);
            res.Pieces = new List<Piece>();
            foreach (Piece piece in Pieces)
            {
                res.Pieces.Add(piece.clone());
            } 
            res.boardEvaluation = this.boardEvaluation;
            return res;
        }

        internal Piece? GetPieceByPos(Position pos)
        {
            Piece? res = null;
            foreach (Piece piece in Pieces)
            {
                if (piece.Pos == pos)
                {
                    res = piece;
                    break;
                }
            }
            return res;
        }

        public int ByTables(bool flip)
        {
            int score = 0;
            foreach (Piece piece in Pieces)
            {
                int ind = piece.Pos.Row * Board.SIZE + piece.Pos.Col;
                // "Flip the rows (rank) while preserving the file" by XORing 56 (0b111000) - the 3 first bits are the row (2**3 = 8)
                ind = piece.Color == PieceColor.White ? ind : ind ^ 56;

                score += piece.GetTableScore(ind);
            }
            return score;
        }

        internal bool qualifyForEndStage()
        {
            bool isQueen = false;
            bool isRook = false;
            foreach (Piece piece in Pieces)
            {
                if (piece is Queen) 
                    isQueen = true;
                if (piece is Rook)
                    isRook = true;
            }
            if (!isQueen) return true;
            if (!isRook && Pieces.Count <= 3) return true;
            return false;
        }
    }
}
    
