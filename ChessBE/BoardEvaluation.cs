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
    /// TODO: This class still has some limitations to be worked out
    /// </summary>
    public class BoardEvaluation
    {
        const int DEPTH = 2;

        /// <summary>
        ///  Calculates the material score
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int MaterialScore(Board state)
        {
            
            Player? autoPlayer = state.getAutoPlayer();
            if (autoPlayer == null)
            {
                return int.MinValue;
            }
            Player manualPlayer = state.getManualPlayer();

            Dictionary<Type, int> autoPlayerPieceCount = autoPlayer.GetPieceCount();
            Dictionary<Type, int> manualPlayerPieceCount = manualPlayer.GetPieceCount();

            return 200 * (autoPlayerPieceCount[typeof(King)] - manualPlayerPieceCount[typeof(King)])
                +9 * (autoPlayerPieceCount[typeof(Queen)] - manualPlayerPieceCount[typeof(Queen)])
                +5 * (autoPlayerPieceCount[typeof(Rook)] - manualPlayerPieceCount[typeof(Rook)])
                +3 * (autoPlayerPieceCount[typeof(Knight)] - manualPlayerPieceCount[typeof(Knight)])
                +3 * (autoPlayerPieceCount[typeof(Bishop)] - manualPlayerPieceCount[typeof(Bishop)])
                +1 * (autoPlayerPieceCount[typeof(Pawn)] - manualPlayerPieceCount[typeof(Pawn)]);
        }

        /// <summary>
        /// Calculates the positional score
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Calculates the mobility score
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
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
        /// Evaluate the whole score 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private int Evaluation(Board state,Move move)
        {
            return 10*MaterialScore(state) + MobilityScore(state);
            //return MaterialScore(state)+MobilityScore(state)+ByTables(state);
        }

        /// <summary>
        /// Returns the best move for the computer player 
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
