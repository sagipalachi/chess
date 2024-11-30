using ChessBE.Pieces;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;

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

            int row = startRow == 0 ? startRow + 1 : startRow - 1;
            for (int i = 0; i < Board.SIZE; i++)
                Pieces.Add(new Pawn(new Position(i, row), Color));


        }
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
        public void RemovePiece(Piece piece)
        {
            Pieces.Remove(piece);
            if (piece is King)
            {
                Board.GetInstance().Checkmate(piece.Color);
            }
        }

        internal void RestorePiece(Piece piece)
        {
            Pieces.Add(piece);
        }

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

        internal List<Move> GetPossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();
            foreach (var piece in Pieces)
            {
                possibleMoves.AddRange(piece.GetPossibleMoves());
            }
            return possibleMoves;
        }

    }
}
    
