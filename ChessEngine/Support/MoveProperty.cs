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
            to.x - from.x;
        public static int DeltaY(Square from, Square to) =>
            to.y - from.y;
        public static int AbsDeltaX(Square from, Square to) =>
            Math.Abs(from.x - to.x);
        public static int AbsDeltaY(Square from, Square to) =>
            Math.Abs(from.y - to.y);
        public static int SignX(Square from, Square to) =>
            Math.Sign(from.x - to.x);
        public static int SignY(Square from, Square to) =>
            Math.Sign(from.y - to.y);
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
