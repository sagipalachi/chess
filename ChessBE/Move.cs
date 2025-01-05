using ChessBE.Pieces;

namespace ChessBE
{
    /// <summary>
    /// Helper class to represent a chess move
    /// </summary>
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