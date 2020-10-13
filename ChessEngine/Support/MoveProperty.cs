using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Domain;

namespace ChessEngine.Support
{
    internal static class MoveProperty
    {
        public static int DeltaX(Square from, Square to) =>
            to.X - from.X;
        public static int DeltaY(Square from, Square to) =>
            to.Y - from.Y;
        public static int AbsDeltaX(Square from, Square to) =>
            Math.Abs(from.X - to.X);
        public static int AbsDeltaY(Square from, Square to) =>
            Math.Abs(from.Y - to.Y);
        public static int SignX(Square from, Square to) =>
            Math.Sign(from.X - to.X);
        public static int SignY(Square from, Square to) =>
            Math.Sign(from.Y - to.Y);
        public static bool IsDiagonalLine(Square from, Square to) =>
            AbsDeltaX(from, to) != 0 &&
            AbsDeltaX(from, to) == AbsDeltaY(from, to);
        public static bool IsVerticalLine(Square from, Square to) =>
            AbsDeltaY(from, to) != 0 &&
            AbsDeltaX(from, to) == 0;
        public static bool IsHorizontalLine(Square from, Square to) =>
            AbsDeltaY(from, to) == 0 &&
            AbsDeltaX(from, to) != 0;
    }
}
