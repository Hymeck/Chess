using ChessEngine;
using ChessEngine.Console;
using ChessEngine.Domain;
using static System.Console;

namespace ChessEngineCorePlayground
{
    class ChessDemo
    {
        static void Main(string[] args)
        {
            var game = new ChessGame();
            MakeMove(ref game, "e2e4");
            MakeMove(ref game, "e7e5");
            MakeMove(ref game, "d1h5");
            MakeMove(ref game, "b8c6");
            MakeMove(ref game, "f1c4");
            MakeMove(ref game, "g8f6");
            while (
                !game.IsCheckmate && 
                !game.IsStalemate)
            {
                PrintGameState(game);
            moveInput:
                Write("\n > ");
                var move = ReadLine();
                if (move == "" || move == "q" || move == "exit")
                    break;
                try
                {
                    MakeMove(ref game, move);
                    Clear();
                }

                catch
                {
                    WriteLine(" Incorrect input");
                    goto moveInput;
                }
            }

            if (game.IsCheckmate)
            {
                PrintBoard(game);
                WriteLine($" {game.ActiveColor.GetReversedColor()} won");
            }

            else if (game.IsStalemate)
            {
                PrintBoard(game);
                WriteLine($" Stalemate");
            }
            ReadKey();
        }

        private static void PrintGameState(ChessGame game)
        {
            PrintBoard(game);

            WriteLine($" Turn     : {game.ActiveColor.ToString()}");
            WriteLine($" Move     : {game.MoveCount}");
            WriteLine($" Check    : {game.IsCheck}");
            WriteLine($" Checkmate: {game.IsCheckmate}");
            WriteLine($" Stalemate: {game.IsStalemate}");
            foreach (var m in game.YieldMoves())
                Write($" {m}");
            WriteLine();
        }

        private static void PrintBoard(ChessGame game)
        {
            WriteLine();
            // PrintWhiteBoard(game.Board);
            if (game.ActiveColor.IsWhite())
                PrintWhiteBoard(game.Board);
            else
                PrintBlackBoard(game.Board);
        }

        private static void MakeMove(ref ChessGame game, string move)
        {
            var (from, to) = ConsoleMoveInput.ParseMove(move);
            game = game.Move(from, to);
        }

        private static void PrintWhiteBoard(Board board)
        {
            for (int y = 7; y > -1; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    var piece = board[x, y].IsNone() ? "." : board[x, y].ToString();
                    var output = " " + piece;
                    Write(output);
                }
                WriteLine($" {y + 1}");
            }
            WriteLine(" a b c d e f g h\n");
        }

        private static void PrintBlackBoard(Board board)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var piece = board[x, y].IsNone() ? "." : board[x, y].ToString();
                    var output = " " + piece;
                    Write(output);
                }
                WriteLine($" {y + 1}");
            }

            WriteLine(" a b c d e f g h\n");
        }
    }
}
