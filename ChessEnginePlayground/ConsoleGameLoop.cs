using System.Diagnostics;
using ChessEngine;
using ChessEngine.Domain;

namespace ChessEnginePlayground
{
    public sealed class ConsoleGameLoop
    {
        private ChessGame game = new ChessGame();
        private readonly GameHandler handler = new GameHandler();

        public void Play()
        {
            // handler.MakeMove(ref game, "e2e4");
            // handler.MakeMove(ref game, "e7e5");
            // handler.MakeMove(ref game, "d1h5");
            // handler.MakeMove(ref game, "b8c6");
            // handler.MakeMove(ref game, "f1c4");
            // handler.MakeMove(ref game, "g8f6");
            while (
                !game.IsCheckmate && 
                !game.IsStalemate)
            {
                handler.PrintGameState(game); 
                handler.MakeMove(ref game);
            }
            End();
        }

        private void End() =>
            handler.PrintEnd(game);
    }
}