using ChessEngine;
using ChessEngine.Console;
using ChessEngine.Domain;
using ChessEnginePlayground;
using static System.Console;

namespace ChessEnginePlayground
{
    class ChessDemo
    {
        static void Main(string[] args)
        {
            var game = new ChessGame("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            var gameLoop = new ConsoleGameLoop(game);
            gameLoop.Play();
            ReadKey();
        }
    }
}
