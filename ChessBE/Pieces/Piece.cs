using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        public bool IsEnemy(Piece? p)
        {
            return p != null && p.Color != Color;
        }

        public virtual List<Position>? GetPotentialPositions()
        { 
            return null; 
        }

        public bool Move(Position targetPos)
        {
            List<Position>? positions = GetPotentialPositions();
            if (positions != null && positions.Any(p => p.isEqual(targetPos)))
            {
                Piece otherPiece = Board.GetInstance().Occupied(targetPos);
                if (otherPiece != null && IsEnemy(otherPiece))
                {
                    Board.GetInstance().RemovePiece(otherPiece);    
                }
                Pos = targetPos;
                return true;
            }
            return false;
        }

    }

    public class Position
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Position(int col, int row)
        {
            Row = row;
            Col = col;
        }

        private bool inBounds()
        {
            return (Row >= 0 && Col >= 0 && Row <= 7 && Col <= 7);
        }

        public bool isEqual(Position p)
        {
            return (p.Row == Row && p.Col == Col);
        }

        public void AddToList(List<Position> posList)
        {
            if (inBounds())
            {
                posList.Add(this);
            }
        }
    }

    public enum PieceColor
    {
        White,
        Black,
    }


  
}
