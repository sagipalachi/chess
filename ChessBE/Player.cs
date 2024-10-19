using ChessBE.Pieces;
using System.Net.NetworkInformation;

namespace ChessBE
{
    public class Player
    {
        public List<Piece> Pieces = new(); 

        public PieceColor Color;
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

            int row  = startRow == 0 ? startRow+1 : startRow-1;
            for (int i = 0; i < Board.SIZE; i++)
                Pieces.Add(new Pawn(new Position(i, row), Color));
         
   
        }

        public Piece? PosOccupied(Position pos)
        {
          foreach(var piece in Pieces)
          {
            Position? pp = piece.Pos;
            if (pp != null && pp.isEqual(pos))
            {
                return piece;
            }
          }
          return null;
        }
        public void RemovePiece(Piece piece) 
        {
            Pieces.Remove(piece);
        }
    }
}