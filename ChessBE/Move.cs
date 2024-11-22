using ChessBE.Pieces;

namespace ChessBE
{
    public class Move
    {
        public Position? src, dest;
        public Piece piece;

        public Move(Piece piece, Position src, Position dest)
        {
            this.piece = piece;
            this.src = src;
            this.dest = dest;
        }


    }
}