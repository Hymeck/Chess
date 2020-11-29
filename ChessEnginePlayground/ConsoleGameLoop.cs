using System.Diagnostics;
using ChessEngine;
using ChessEngine.Domain;

namespace ChessEnginePlayground
{
    public sealed class ConsoleGameLoop
    {
        private ChessGame game;
        private readonly GameHandler handler = new GameHandler();

        public ConsoleGameLoop() =>
            game = new ChessGame();
        public ConsoleGameLoop(ChessGame game) => 
            this.game = game;

        public void Play()
        {
            while (
                !game.IsCheckmate && 
                !game.IsStalemate)
            {
                handler.PrintGameState(game); 
                var result = handler.MakeMove(ref game);
                if (result == -1)
                    break;
            }
            End();
        }

        private void End() =>
            handler.PrintEnd(game);
    }
}