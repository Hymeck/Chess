using ChessEngine;
using ChessEngine.Console;
using ChessEngine.Domain;
using ChessEngine.Exceptions;
using static System.Console;

namespace ChessEnginePlayground
{
    public sealed class GameHandler
    {
        public int MakeMove(ref ChessGame game)
        {
            moveInput:
            Write("\n > ");
            var move = ReadLine();
            if (move == "" || move == "q" || move == "exit")
                return -1;
            try
            {
                MakeMove(ref game, move);
                Clear();
                return 0;
            }

            catch (ChessGameException e)
            {
                WriteLine($" {e.Message}");
                goto moveInput;
            }
            
            catch
            {
                WriteLine(" Invalid input");
                goto moveInput;
            }
        }
        public void MakeMove(ref ChessGame game, string move)
        {
            var (from, to) = MoveInputParser.ParseMove(move);
            game = game.Move(from, to);
        }

        public void PrintEnd(ChessGame game)
        {
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

            else
                WriteLine(" Aborted");
        }
        public void PrintGameState(ChessGame game)
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
        
        public void PrintBoard(ChessGame game)
        {
            if (game.ActiveColor.IsWhite())
                PrintWhiteBoard(game.Board);
            else
                PrintBlackBoard(game.Board);
        }
        public void PrintWhiteBoard(Board board)
        {
            for (int y = 7; y > -1; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    var piece = board[x, y].IsNone() ? "." : board[x, y].Name;
                    var output = " " + piece;
                    Write(output);
                }
                WriteLine($" {y + 1}");
            }
            WriteLine(" a b c d e f g h\n");
        }
        
        public void PrintBlackBoard(Board board)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var piece = board[x, y].IsNone() ? "." : board[x, y].Name;
                    var output = " " + piece;
                    Write(output);
                }
                WriteLine($" {y + 1}");
            }

            WriteLine(" a b c d e f g h\n");
        }
    }
}