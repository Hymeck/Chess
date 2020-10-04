using ChessEngine.Domain;
using System;

namespace ChessEngine.Console
{
    public static class ConsoleMoveInput
    {
        public static (Square from, Square to) ParseMove(string move)
        {
            if (move.Length < 4)
                throw new FormatException("Input length must be 4");
            if (!IsValidLetter(move[0]) || !IsValidLetter(move[2]))
                throw new FormatException("Letter from 'a' to 'h'");
            if (!IsValidDigit(move[1]) || !IsValidDigit(move[3]))
                throw new FormatException("Digit from '1' to '8'");
            // todo: check promotion letter
            return (SquareMethods.FromString(move.Substring(0, 2)),
                SquareMethods.FromString(move.Substring(2, 2)));
        }

        private static bool IsValidLetter(char c) =>
            c >= 'a' && c <= 'h';
        private static bool IsValidDigit(char d) =>
            d >= '1' && d <= '8';
    }
}
