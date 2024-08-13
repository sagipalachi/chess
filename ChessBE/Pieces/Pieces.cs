using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE.Pieces
{
    public abstract class Piece
    {
        public Position? Pos { get; set; }
        public PieceColor Color = PieceColor.Black;

        public Piece(Position? pos, PieceColor color)
        {
            Pos = pos;
            Color = color;
        }

        public abstract void Move();
    }

    public class Position
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }

    public enum PieceColor
    {
        White,
        Black,
    }


    public class King : Piece
    {
        public King(Position? pos, PieceColor color) : base(pos, color)
        {
        }

        public override void Move()
        {

        }
    }
    public class Queen : Piece
    {
        public Queen(Position? pos, PieceColor color) : base(pos, color)
        {
        }
        public override void Move()
        {

        }
    }
    public class Rook : Piece
    {
        public Rook(Position? pos, PieceColor color) : base(pos, color)
        {
        }
        public override void Move()
        {

        }
    }
    public class Knight : Piece
    {
        public Knight(Position? pos, PieceColor color) : base(pos, color)
        {
        }
        public override void Move()
        {

        }
    }
    public class Bishop : Piece
    {
        public Bishop(Position? pos, PieceColor color) : base(pos, color)
        {
        }
        public override void Move()
        {

        }
    }

}
