using ChessBE.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ChessBE
{
    /// <summary>
    /// The "AI Engine" - evaluates the best move for a player
    /// </summary>
    internal class BoardEvaluation
    {
        const int DEPTH = 3;
        //public int Eval()
        //{
        //    return (TableScore() + MatirialScore()) * Board.GetInstance().GetTurnValue();
        //}

        /// <summary>
        /// Calculates the mobility score - not yet implemented
        /// </summary>
        /// <returns></returns>
        

        /// <summary>
        /// Calculates the material score
        /// </summary>
        /// <returns></returns>
        public static int MatirialScore(Board state)
        {
            
            Player? autoPlayer = state.getAutoPlayer();
            if (autoPlayer == null)
            {
                return int.MinValue;
            }
            Player manualPlayer = state.getManualPlayer();
            return 200 * (autoPlayer.GetPieceCount(typeof(King)) - manualPlayer.GetPieceCount(typeof(King)))
                +9 * (autoPlayer.GetPieceCount(typeof(Queen)) - manualPlayer.GetPieceCount(typeof(Queen)))
                +4 * (autoPlayer.GetPieceCount(typeof(Rook)) - manualPlayer.GetPieceCount(typeof(Rook)))
                +3 * (autoPlayer.GetPieceCount(typeof(Knight)) - manualPlayer.GetPieceCount(typeof(Knight)))
                +3 * (autoPlayer.GetPieceCount(typeof(Bishop)) - manualPlayer.GetPieceCount(typeof(Bishop)))
                +1 * (autoPlayer.GetPieceCount(typeof(Pawn)) - manualPlayer.GetPieceCount(typeof(Pawn)));
        }

        public static int ByTables(Board state)
        {
            Player? autoPlayer = state.getAutoPlayer();
            if (autoPlayer == null)
            {
                return int.MinValue;
            }
            Player manualPlayer = state.getManualPlayer();
            return autoPlayer.ByTables(false) - manualPlayer.ByTables(true);
        }

        private int MobilityScore(Board state)
        {
            Player? autoPlayer = state.getAutoPlayer();
            if (autoPlayer == null)
            {
                return int.MinValue;
            }
            Player manualPlayer = state.getManualPlayer();
            return autoPlayer.GetPossibleMoves().Count-manualPlayer.GetPossibleMoves().Count;
        }


        /// <summary>
        /// Evaluate the whole score (materal + mobility) 
        /// Mobility is yet to be implemented
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private int Evaluation(Board state,Move move)
        {
            return MatirialScore(state)+MobilityScore(state)+ByTables(state);
        }

        /// <summary>
        /// Returns the best move for the player whose turn it is
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public Move  BestMove(Board state)
        {
            Move best = null;
            int bestValue = int.MinValue;
            int valueCurrent = 0;
            List <Move> moves = state.GetPossibleMoves(state.blackPlayer);
            for (int i = 0; i < moves.Count; i++)
            {
                DoMove(moves[i]);
                valueCurrent = alphaBeta(state, DEPTH-1, bestValue, int.MaxValue, false, moves[i]) ;
                UndoMove(moves[i]);
                if (valueCurrent > bestValue)
                {
                    bestValue = valueCurrent;
                    best = moves[i];
                }
            }
            return best;      
        }

        /// <summary>
        /// Alpha Beta algorithm implementation to find the best state in the tree
        /// </summary>
        /// <param name="state"></param>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int alphaBeta(Board state, int depth, int alpha, int beta , bool max,Move move)
        {
            if (depth == 0)
            {
                    return Evaluation(state,move);
            }
            Player? autoPlayer = state.getAutoPlayer();
            if (autoPlayer == null)
            {
                return int.MinValue;
            }
            Player manualPlayer = state.getManualPlayer();
            Player player = max ? autoPlayer : manualPlayer;
            List<Move> moves = state.GetPossibleMoves(player);
            int valueCurrent = max ? int.MaxValue : int.MinValue;
            for (int i = 0; i < moves.Count; i++)
            {
                DoMove(moves[i]);
                valueCurrent = alphaBeta(state, depth - 1, alpha, beta, !max, moves[i]);
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

        /// <summary>
        /// Do a "eval move" for calculating the score of the "eval state" by alpha beta
        /// </summary>
        /// <param name="move"></param>
        private void DoMove(Move move)
        {
            List<Position> dummy = new List<Position>();
            move.piece.Move(move.dest, false, out dummy);
        }

        /// <summary>
        /// Undo the "eval move" to get back to the previous state in the evaluation (alpha beta tree)
        /// </summary>
        /// <param name="move"></param>
        private void UndoMove(Move move)
        {
            move.piece.UndoMove(move.src);
        }
    }
}
