using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE
{
    public class Board
    {
        private List<Piece> pieces = new List<Piece>();
        public Board()
        {
            setupPieces();
        }

        private void setupPieces()
        {
            pieces.Add(new Rook(new Position(0, 0), PieceColor.Black));
            pieces.Add(new Knight(new Position(1, 0), PieceColor.Black));
            pieces.Add(new Bishop(new Position(2, 0), PieceColor.Black));
            pieces.Add(new Queen(new Position(3, 0), PieceColor.Black));
            pieces.Add(new King(new Position(4, 0), PieceColor.Black));
            pieces.Add(new Bishop(new Position(5, 0), PieceColor.Black));
            pieces.Add(new Knight(new Position(6, 0), PieceColor.Black));
            pieces.Add(new Rook(new Position(7, 0), PieceColor.Black));

            pieces.Add(new Rook(new Position(0, 7), PieceColor.White));
            pieces.Add(new Knight(new Position(1, 7), PieceColor.White));
            pieces.Add(new Bishop(new Position(2, 7), PieceColor.White));
            pieces.Add(new Queen(new Position(3, 7), PieceColor.White));
            pieces.Add(new King(new Position(4, 7), PieceColor.White));
            pieces.Add(new Bishop(new Position(5, 7), PieceColor.White));
            pieces.Add(new Knight(new Position(6, 7), PieceColor.White));
            pieces.Add(new Rook(new Position(7, 7), PieceColor.White));

            for (int i = 0; i < 8; i++)
            {
                pieces.Add(new Pawn(new Position(i, 1), PieceColor.Black));
                pieces.Add(new Pawn(new Position(i, 6), PieceColor.White));
            }
        }
        public List<Piece> GetPieces()
        {
            return pieces;
        }
    }
}
