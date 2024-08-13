using ChessBE.Pieces;

namespace ChessBE
{
    public class Player
    {
        Dictionary<int, Piece> pieces = new Dictionary<int, Piece>();   //  int - position 0 - 63

        public Player(int startRow)
        {
            PieceColor color = startRow == 0 ? PieceColor.Black : PieceColor.White;
            pieces.Add(startRow * Board.SIZE + 0, new Rook(new Position(0, startRow), color));
            pieces.Add(startRow * Board.SIZE + 1, new Knight(new Position(1, startRow), color));
            pieces.Add(startRow * Board.SIZE + 2, new Bishop(new Position(2, startRow), color));
            pieces.Add(startRow * Board.SIZE + 3, new Queen(new Position(3, startRow), color));
            pieces.Add(startRow * Board.SIZE + 4, new King(new Position(4, startRow), color));
            pieces.Add(startRow * Board.SIZE + 5, new Bishop(new Position(5, startRow), color));
            pieces.Add(startRow * Board.SIZE + 6, new Knight(new Position(6, startRow), color));
            pieces.Add(startRow * Board.SIZE + 7, new Rook(new Position(7, startRow), color));

            int row  = startRow == 0 ? startRow+1 : startRow-1;
            for (int i = 0; i < Board.SIZE; i++)
                pieces.Add(row * Board.SIZE + i, new Pawn(new Position(i, row), color));
         
   
        }

        public Dictionary<int, Piece> Pieces { get => pieces; set => pieces = value; }
    }
}