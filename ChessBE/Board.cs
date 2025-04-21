using ChessBE.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ChessBE
{
    /// <summary>
    /// Enumerated type to indicate a Check status
    /// </summary>
    public enum CheckStatus
    {
        None,
        White,
        Black,
    }

    /// <summary>
    /// Enumerated type for indicating a Checkmate status
    /// </summary>
    public enum CheckmateStatus
    {
        None,
        White,
        Black,
    }

    /// <summary>
    /// A singleton class representing the chess board - this is the "main" backend class
    /// </summary>
    public class Board
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static Board? _instance = null;
        public static int SIZE = 8;
        public Player blackPlayer, whitePlayer;
        private Player? backupBlackPlayer, backupWhitePlayer;
        private PieceColor backupColor;
        private Player turnPlayer;
        public CheckStatus BoardCheckStatus { get; set; }
        public CheckmateStatus BoardCheckmateStatus { get; set; }

        /// <summary>
        /// Singleton pattern - get the singleton instance of the board
        /// </summary>
        /// <returns></returns>
        public static Board GetInstance()
        {
            if (_instance == null)
                _instance = new Board();
            return _instance;
        }
        /// <summary>
        /// Private constructor to initialize the singleton instance
        /// </summary>
        private Board()
        {
            blackPlayer = new Player(0);
            whitePlayer = new Player(7);
            turnPlayer = whitePlayer;
            BoardCheckStatus = CheckStatus.None;
            BoardCheckmateStatus = CheckmateStatus.None;
            blackPlayer.SetAutoMode(true);
            Logger.Info("New Board Created");
            backupBlackPlayer = null;
            backupWhitePlayer = null;
        }

        /// <summary>
        /// Returns true if position is occupied by a piece and falLs if its not
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Piece Occupied(Position pos)
        {
            Piece? p = blackPlayer.PosOccupied(pos);
            if (p == null)
                p = whitePlayer.PosOccupied(pos);
            return p;
        }

        /// <summary>
        /// Return true if the game qualifies to be at the end stage
        /// </summary>
        /// <returns></returns>
        public bool isEndStage()
        {
            if (blackPlayer.qualifyForEndStage()  && whitePlayer.qualifyForEndStage())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns 1 if its white's tune and -1 if its black's
        /// </summary>
        /// <returns></returns>
        public int GetTurnValue()
        {
            return (turnPlayer == whitePlayer) ? 1 : -1;

        }
        /// <summary>
        /// Pass turn between players, if the player that plays is in auto move 
        /// (meaning it is played by the computer) then return true when the auto
        /// play move is completed. Otherwise, return false (turn should be played
        /// by a human user)
        /// </summary>
        /// <param name="oldPositions"></param>
        /// <returns></returns>
        public bool passTurn(out List<Position> oldPositions)
        {
            blackPlayer.ClearLastCapturedPieces();
            whitePlayer.ClearLastCapturedPieces();
            oldPositions = null;
            turnPlayer = (turnPlayer.Color == PieceColor.White ? blackPlayer : whitePlayer);
            if (turnPlayer.IsAutoMode())
            {
                Logger.Info(whitePlayer.ToString());
                Logger.Info(blackPlayer.ToString());
                turnPlayer.DoAutoMove(out oldPositions);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Used from auto moves - just switch the turn player
        /// </summary>
        public void switchTurn()
        {
            turnPlayer = (turnPlayer == whitePlayer ? blackPlayer : whitePlayer);
        }

        /// <summary>
        /// Return the player played by the computer
        /// </summary>
        /// <returns></returns>
        public Player? getAutoPlayer()
        {
            if (blackPlayer.IsAutoMode())
            {
                return blackPlayer;
            }
            if (whitePlayer.IsAutoMode())
            {
                return whitePlayer;
            }
            return null;
        }

        /// <summary>
        /// Return the player played by the user
        /// </summary>
        /// <returns></returns>
        public Player getManualPlayer()
        {
            if (blackPlayer.IsAutoMode())
            {
                return whitePlayer;
            }
            return blackPlayer;
        }

        /// <summary>
        /// Check whose turn it is to play
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public bool CheckTurn(Piece piece)
        {
            return piece.Color == turnPlayer.Color;
        }

        /// <summary>
        /// Return which color to play
        /// </summary>
        /// <returns>The turn color as string</returns>
        public string GetTurnColor()
        {
            string res = "white";
            if (turnPlayer.Color == PieceColor.Black)
            {
                res = "black";
            }
            return res;
        }


        /// <summary>
        /// Remove a piece from the board since it was captured
        /// </summary>
        /// <param name="piece"></param>
        public bool RemovePiece(Piece piece)
        {
            bool res = false;
            if (piece.Color == PieceColor.White)
                res = whitePlayer.RemovePiece(piece);
            else
                res = blackPlayer.RemovePiece(piece);
            return res;
        }

        /// <summary>
        /// Update the check status of the board
        /// </summary>
        public void UpdateCheckStatus()
        {
            BoardCheckStatus = CheckStatus.None;

            List<Piece> pieces = new List<Piece>();

            pieces.AddRange(whitePlayer.Pieces);
            pieces.AddRange(blackPlayer.Pieces);

            foreach (Piece piece in pieces)
            {
                piece.GetPotentialPositions();
            }
        }

        /// <summary>
        /// Return the position of the king that has the input Color
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Position? GetKingPos(PieceColor p)
        {
            if (p == PieceColor.White)
            {
                return whitePlayer.GetKingPos();
            }
            return blackPlayer.GetKingPos();
        }

        /// <summary>
        /// Set the chekcmate status
        /// </summary>
        /// <param name="color"></param>
        public void Checkmate(PieceColor color)
        {
            if (color == PieceColor.White)
                BoardCheckmateStatus = CheckmateStatus.White;
            else 
                BoardCheckmateStatus = CheckmateStatus.Black;
        }

        /// <summary>
        /// Reset the board - when starting a new game
        /// </summary>
        public static void ResetBoard()
        {
            _instance = null;
            GetInstance();
        }


        /// <summary>
        /// Get all possible moves for the turn player (the player whose turn it is to play)
        /// </summary>
        /// <returns></returns>
        public List<Move> GetPossibleMoves(Player player)
        {
            return player.GetPossibleMoves();
        }

        /// <summary>
        /// True if its white's turn, false if its black's
        /// </summary>
        /// <returns></returns>
        public bool isTurnPlayerWhite()
        {
            return turnPlayer.Color == PieceColor.White;
               
        }

        /// <summary>
        /// Restore a piece that was captured - used by the "AI Play" can be used
        /// for undo in the future
        /// </summary>
        /// <param name="lastCapturedEnemyPiece"></param>
        public void RestorePiece(Piece lastCapturedEnemyPiece)
        {
            if (lastCapturedEnemyPiece != null)
            {
                this.BoardCheckmateStatus = CheckmateStatus.None;
                if (lastCapturedEnemyPiece.Color == PieceColor.White) 
                {
                    whitePlayer.RestorePiece(lastCapturedEnemyPiece);
                }
                else 
                {
                    blackPlayer.RestorePiece(lastCapturedEnemyPiece);
                }
            }
        }

        /// <summary>
        /// Set the game mode to auto play - playing against the computer
        /// </summary>
        /// <param name="automode"></param>
        public void SetAutoPlay(bool automode)
        {
            if (!automode)
            {
                blackPlayer.SetAutoMode(false);
                whitePlayer.SetAutoMode(false);
            }
            else
            {
                if (GetTurnValue() == 1)
                    blackPlayer.SetAutoMode(automode);
                else
                    whitePlayer.SetAutoMode(automode);
            }
        }

        /// <summary>
        /// Convert a pawn to queen when a pawn reaches the other
        /// side of the board.
        /// Known limitation: we always convert to queen (no option to convert to 
        /// a different type of piece)
        /// </summary>
        /// <param name="pawn"></param>
        public void ConvertPawnToQueen(Pawn pawn)
        {
            if (pawn.Color == PieceColor.White)
            {
                whitePlayer.ConvertPawnToQueen(pawn);
            }
            else
            {
                blackPlayer.ConvertPawnToQueen(pawn);
            }
        }

        /// <summary>
        /// Return the chess piece located in a given position
        /// </summary>
        /// <param name="pos">The position</param>
        /// <returns></returns>
        public Piece? GetPieceByPos(Position pos)
        {
            Piece? piece = whitePlayer.GetPieceByPos(pos);
            if (piece == null)
            {
                piece = blackPlayer.GetPieceByPos(pos);
            }
            return piece;
        }
    }
}
