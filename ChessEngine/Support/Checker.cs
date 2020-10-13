using System;
using ChessEngine.Support;
using ChessEngine.Domain;

namespace ChessEngine.Support
{
    internal static class Checker
    {
        public static bool AreSquaresOnBoard(Square from, Square to) =>
            from.IsOnBoard() && to.IsOnBoard();

        public static bool AreSquaresEqual(Square from, Square to) =>
            from == to;
        
        public static bool IsValidColors(Color activeColor, Piece activePiece, Piece enemyPiece) =>
            activePiece.Color == activeColor &&
            activePiece.Color != enemyPiece.Color;

        public static bool IsAnyPieceBetween(Board board, Square from, Square to)
        {
            //if 
            var stepX = MoveProperty.SignX(from, to);
            var stepY = MoveProperty.SignY(from, to);

            var tempFrom = new Square(from.X, from.Y);
            while (true)
            {
                // beware: plus or minus step*
                tempFrom = new Square(tempFrom.X - stepX, tempFrom.Y - stepY);

                if (tempFrom == to)
                    return false;
                if (board[tempFrom].IsNone())
                    continue;

                return true;
            }
        }

        // suppose that path from is vertical/horizontal/diagonal line
        public static bool CanMoveLine(Board board, Square from, Square to)
        {
            var stepX = MoveProperty.SignX(from, to);
            var stepY = MoveProperty.SignY(from, to);

            var tempFrom = new Square(from.X, from.Y);
            while (true)
            {
                tempFrom = new Square(tempFrom.X - stepX, tempFrom.Y - stepY);

                if (tempFrom == to)
                    return true;
                if (board[tempFrom].IsNone())
                    continue;

                return false;
            }
        }

        public static bool CanMoveLineary(bool linePredicate,
            Board board, Square from, Square to) =>
            !linePredicate ?
            false :
            CanMoveLine(board, from, to);
    }
}
