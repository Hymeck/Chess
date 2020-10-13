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
            ConsoleGameLoop gameLoop = new ConsoleGameLoop();
            gameLoop.Play();
            ReadKey();
        }
    }
}
