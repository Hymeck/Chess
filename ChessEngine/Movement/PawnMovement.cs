using ChessEngineTry2.Domain;

namespace ChessEngineTry2.Rules
{
    internal static class PawnMovement
    {
        public static bool CanPawnMove(Color pawnColor, Board board, Square from, Square to) =>
            CanPawnForward(pawnColor, board, from, to) ||
            CanPawnPush(pawnColor, board, from, to) ||
            CanPawnAttack(pawnColor, board, from, to);

        private static bool CanPawnForward(Color pawnColor, Board board, Square from, Square to)
        {
            // white: a1 -> a2; from.y - to.y == 1; from.x == to.x
            // black: a2 -> a1; from.y - to.y == -1; from.x == to.x
            int step = GetPawnStep(pawnColor);
            return
                MoveProperty.DeltaX(from, to) == 0 &&
                MoveProperty.DeltaY(from, to) == step &&
                !board[to].HasValue;
        }

        private static bool CanPawnPush(Color pawnColor, Board board, Square from, Square to)
        {
            // white: a2 -> a4; from.y - to.y == 2; from.x == to.x
            // black: a7 -> a5; from.y - to.y == -2; from.x == to.x
            int row = GetPawnStartRow(pawnColor);
            int step = GetPawnStep(pawnColor);
            return
                MoveProperty.DeltaX(from, to) == 0 &&
                MoveProperty.DeltaY(from, to) == 2 * step &&
                row == from.y &&
                !board[to].HasValue &&
                !board[new Square(from.x, from.y + step)].HasValue;
        }

        private static bool CanPawnAttack(Color pawnColor, Board board, Square from, Square to)
        {
            // white: b2 -> a3 || c3; from.y - to.y == 1; |from.x - to.x| == 1
            // black: b7 -> a6 || c6; from.y - to.y == -1; |from.x == to.x| == 1
            int step = GetPawnStep(pawnColor);
            return
                MoveProperty.AbsDeltaX(from, to) == 1 &&
                MoveProperty.DeltaY(from, to) == step &&
                board[to].HasValue;
        }

        private static int GetPawnStep(Color pawnColor) =>
            pawnColor.IsWhite() ? 1 : -1;

        private static int GetPawnStartRow(Color pawnColor) =>
            pawnColor.IsWhite() ? 1 : 6;
    }
}
