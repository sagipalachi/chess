using ChessBE.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE
{
    public enum CheckStatus
    {
        None,
        White,
        Black,
    }

    public enum CheckmateStatus
    {
        None,
        White,
        Black,
    }

    public class Board
    {
        private static Board? _instance = null;
        public static int SIZE = 8;
        public Player blackPlayer, whitePlayer;
        private Player turnPlayer;
        public CheckStatus BoardCheckStatus { get; set; }
        public CheckmateStatus BoardCheckmateStatus { get; set; }


        public static Board GetInstance()
        {
            if (_instance == null)
                _instance = new Board();
            return _instance;
        }
        private Board()
        {
            blackPlayer = new Player(0);
            whitePlayer = new Player(7);
            turnPlayer = whitePlayer;
            BoardCheckStatus = CheckStatus.None;
            BoardCheckmateStatus = CheckmateStatus.None;
            blackPlayer.SetAutoMode(true);
        }
        public Piece Occupied(Position pos)
        {
            Piece? p = blackPlayer.PosOccupied(pos);
            if (p == null)
                p = whitePlayer.PosOccupied(pos);
            return p;
        }
        public int GetTurnValue()
        {
            return (turnPlayer == whitePlayer) ? 1 : -1;

        }
        // returns true if an auto move was done, otherwise false
        public bool passTurn(out List<Position> oldPositions)
        {
            oldPositions = null;
            turnPlayer = (turnPlayer == whitePlayer ? blackPlayer : whitePlayer);
            if (turnPlayer.IsAutoMode())
            {
                turnPlayer.DoAutoMove(out oldPositions);
                return true;
            }
            return false;
        }

        public bool CheckTurn(Piece piece)
        {
            return piece.Color == turnPlayer.Color;
        }

        public void RemovePiece(Piece piece)
        {
            if (piece.Color == PieceColor.White)
                whitePlayer.RemovePiece(piece);
            else
                blackPlayer.RemovePiece(piece);
        }

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
        public Position? GetKingPos(PieceColor p)
        {
            if (p == PieceColor.White)
            {
                return whitePlayer.GetKingPos();
            }
            return blackPlayer.GetKingPos();
        }
        public void Checkmate(PieceColor color)
        {
            if (color == PieceColor.White)
                BoardCheckmateStatus = CheckmateStatus.White;
            else 
                BoardCheckmateStatus = CheckmateStatus.Black;
        }

        public static void ResetBoard()
        {
            _instance = null;
            GetInstance();
        }

    

        public List<Move>  GetPossiableMoves()
        {
            return turnPlayer.GetPossibleMoves();
        }
        public bool isTurnPlayerWhite()
        {
            if (turnPlayer.Color == PieceColor.White)
                return true;
            return false;
        }
        public void RestorePiece(Piece? lastCapturedEnemyPiece)
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

        public Player Player
        {
            get => default;
            set
            {
            }
        }
    }
}
