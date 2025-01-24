using ChessBE.Pieces;
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
            Move bestMove = boardEvaluation.BestMove(Board.GetInstance());
            bestMove.piece.Move(bestMove.dest, true, out oldPositions);
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
    }
}
    
