﻿using ChessBE.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBE
{
    internal class BoardEvaluation
    {
        const int DEPTH = 3;
        //public int Eval()
        //{
        //    return (MobilityScore() + MetirialScore()) * Board.GetInstance().GetTurnValue();
        //}
        public int MobilityScore()
        {
            return 0;
        }
        public static int MetirialScore()
        {

            return 0;
        }
        private int Evaluation(Board state)
        {
            return MetirialScore();
        }

        public Move  BestMove(Board state)
        {
            Move best = null;
            int bestValue = int.MinValue;
            int valueCurrent = 0;
            List<Move> moves = state.GetPossiableMoves();
            for (int i = 0; i < moves.Count; i++)
            {
                DoMove(moves[i]);
                valueCurrent = alphaBeta(state, DEPTH, bestValue, int.MaxValue);
                UndoMove(moves[i]);
                if (valueCurrent > bestValue)
                {
                    bestValue = valueCurrent;
                    best = moves[i];
                }

            }
            return best;
        }

       
        private int alphaBeta(Board state, int depth, int alpha, int beta)
        {
            if (depth == 0)
            {
                return Evaluation(state);
            }
            int valueCurrent=0;
            List<Move> moves = state.GetPossiableMoves();
            for (int i = 0; i < moves.Count; i++)
            {
                DoMove(moves[i]);
                valueCurrent = -alphaBeta(state, depth - 1, -beta, -alpha);
                UndoMove(moves[i]);
                alpha = Math.Max(alpha, valueCurrent);
                if (alpha >= beta)
                    break;            //  (*cut - off *)
            }
            return valueCurrent;
        }

        private void DoMove(Move move)
        {
            List<Position> dummy = new List<Position>();
            move.piece.Move(move.dest, out dummy);
        }

        private void UndoMove(Move move)
        {
            move.piece.UndoMove(move.src);
        }
    }
}
