using ChessBE.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            Player b = Board.GetInstance().blackPlayer;
            Player w = Board.GetInstance().whitePlayer;
            return 200 * (b.GetPieceCount(typeof(King)) - w.GetPieceCount(typeof(King)))
                +9 * (b.GetPieceCount(typeof(Queen)) - w.GetPieceCount(typeof(Queen)))
                +4 * (b.GetPieceCount(typeof(Rook)) - w.GetPieceCount(typeof(Rook)))
                +3 * (b.GetPieceCount(typeof(Knight)) - w.GetPieceCount(typeof(Knight)))
                +3 * (b.GetPieceCount(typeof(Bishop)) - w.GetPieceCount(typeof(Bishop)))
                +1 * (b.GetPieceCount(typeof(Pawn)) - w.GetPieceCount(typeof(Pawn)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
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
                valueCurrent = alphaBeta(state, DEPTH-1, bestValue, int.MaxValue, false) ;
                UndoMove(moves[i]);
                if (valueCurrent > bestValue)
                {
                    bestValue = valueCurrent;
                    best = moves[i];
                }
            }
            return best;      
        }

       
        private int alphaBeta(Board state, int depth, int alpha, int beta , bool max)
        {
            if (depth == 0)
            {
                    return Evaluation(state);
            }
            List<Move> moves = state.GetPossiableMoves();
            int valueCurrent = max ? int.MinValue : int.MaxValue;
            for (int i = 0; i < moves.Count; i++)
            {
                DoMove(moves[i]);
                valueCurrent = alphaBeta(state, depth - 1, alpha, beta, !max);
                UndoMove(moves[i]);
                if (max)
                {
                    alpha = Math.Max(alpha, valueCurrent);
                    if (valueCurrent >= beta)
                        break;            //  (*cut - off *)
                }
                else
                {
                    beta = Math.Min(beta, valueCurrent);
                    if (valueCurrent <= alpha)
                        break;            //  (*cut - off *)
                }                                        
            }
            return valueCurrent;
        }

        private void DoMove(Move move)
        {
            List<Position> dummy = new List<Position>();
            move.piece.Move(move.dest, false, out dummy);
        }

        private void UndoMove(Move move)
        {
            move.piece.UndoMove(move.src);
        }
    }
}
