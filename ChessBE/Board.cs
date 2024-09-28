﻿using ChessBE.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE
{
    public class Board
    {
        private static Board? _instance = null;
        public static int SIZE = 8;
        public Player blackPlayer, whitePlayer;
        private Player turnPlayer;

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

            //Piece piece = blackPlayer.Pieces[row * Board.SIZE + col];
        }
        public Piece Occupied(Position pos)
        {
            Piece? p = blackPlayer.PosOccupied(pos);
            if (p == null)
                p = whitePlayer.PosOccupied(pos);
            return p;
        }

        public void passTurn()
        {
            turnPlayer = (turnPlayer == whitePlayer ? blackPlayer : whitePlayer);
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
    }
}
