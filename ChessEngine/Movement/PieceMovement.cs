using ChessEngine.Constants;
using ChessEngine.Domain;
using ChessEngine.Support;

namespace ChessEngine.Movement
{
    internal static class KnightMovement
    {
        public static MoveSummary CanKnightMove(Board board, Square from, Square to)
        {
            var isSameColor = board[from].color == board[to].color;
            var isCapturing = board[to].IsNone() ?
                false :
                !isSameColor;
            var isMovePossible =
                !isSameColor &&
                MoveProperty.AbsDeltaX(from, to) *
                MoveProperty.AbsDeltaY(from, to) == 2;
            return MoveSummaryBuilder.DefaultMoveSummary(isCapturing, isMovePossible, to);
        }
    }

    internal static class BishopMovement
    {
        public static MoveSummary CanBishopMove(Board board, Square from, Square to)
        {
            var isSameColor = board[from].color == board[to].color;
            var isCapturing = board[to].IsNone() ?
                false :
                !isSameColor;
            var isMovePossible =
                !isSameColor &&
                Checker.CanMoveLineary(
                    MoveProperty.IsDiagonalLine(from, to), 
                    board, from, to);
            return MoveSummaryBuilder.DefaultMoveSummary(isCapturing, isMovePossible, to);
        }
    }

    internal static class RookMovement
    {
        public static MoveSummary CanRookMove(Board board, Square from, Square to)
        {
            //throw new NotImplementedException("CanRookMove");
            var piece = board[from];
            var isSameColor = board[from].color == board[to].color;
            var isCapturing = board[to].IsNone() ?
                false :
                !isSameColor;
            var isMovePossible =
                !isSameColor &&
                Checker.CanMoveLineary(
                MoveProperty.IsHorizontalLine(from, to) ||
                MoveProperty.IsVerticalLine(from, to),
                board, from, to);

            var isWhite = piece.color.IsWhite();
            var startQueensideSquare = isWhite ?
                WhiteCastlingSquares.QueensideRookStartSquare :
                BlackCastlingSquares.QueensideRookStartSquare;

            var startKingsideSquare = isWhite ?
                WhiteCastlingSquares.KingsideRookStartSquare :
                BlackCastlingSquares.KingsideRookStartSquare;

            var isQueensideRookMoving =
                from == startQueensideSquare;
            var isKingsideRookMoving =
                from == startKingsideSquare;

            return MoveSummaryBuilder.RookSummary(
                isCapturing, isMovePossible,
                isQueensideRookMoving, isKingsideRookMoving, to);
        }
    }

    internal static class QueenMovement
    {
        public static MoveSummary CanQueenMove(Board board, Square from, Square to)
        {
            var isSameColor = board[from].color == board[to].color;
            var isCapturing = board[to].IsNone() ?
                false :
                !isSameColor;

            var isMovePossible =
                !isSameColor && 
                Checker.CanMoveLineary(
                MoveProperty.IsHorizontalLine(from, to) ||
                MoveProperty.IsVerticalLine(from, to) ||
                MoveProperty.IsDiagonalLine(from, to),
                board, from, to);
            return MoveSummaryBuilder.DefaultMoveSummary(isCapturing, isMovePossible, to);
        }
    }

    internal static class KingMovement
    {
        public static MoveSummary CanKingMove(ChessGame game, Color kingColor, Board board, Square from, Square to)
        {
            var isSameColor = board[from].color == board[to].color;
            var isCapturing = board[to].IsNone() ?
                false :
                !isSameColor;
            var canDefaultMove = CanDefaultMove(from, to, isSameColor);
            if (canDefaultMove)
                return MoveSummaryBuilder.KingSummary(isCapturing, true, false, false, to);
            return 
                CanCastle(game, kingColor, board, from, to);
        }

        private static bool CanDefaultMove(Square from, Square to, bool isSameColor) =>
            !isSameColor &&
            MoveProperty.AbsDeltaX(from, to) <= 1 &&
            MoveProperty.AbsDeltaY(from, to) <= 1;

        private static MoveSummary CanCastle(ChessGame game, Color kingColor, Board board, Square from, Square to)
        {
            var isHorizontalLine = MoveProperty.IsHorizontalLine(from, to);
            if (isHorizontalLine)
            {
                var isAnyPieceBetween = Checker.IsAnyPieceBetween(board, from, to);
                if (!isAnyPieceBetween)
                {
                    var isKingWhite = kingColor.IsWhite();
                    var hasKingMoved =
                        isKingWhite ?
                        game.HasWhiteKingMoved :
                        game.HasBlackKingMoved;

                    if (hasKingMoved)
                        return
                            MoveSummaryBuilder.DefaultMoveSummary(false, false, Square.NoneSquare);

                    var kingStartSquare = GetKingStartSquare(isKingWhite);
                    if (kingStartSquare == from)
                    {
                        var isQueensideDirection = IsQueensideDirection(isKingWhite, to);
                        var isKingsideDirection = IsKingsideDirection(isKingWhite, to);
                        Square rookStartSquare;
                        bool hasRookMoved;
                        if (isQueensideDirection)
                        {
                            rookStartSquare = GetQueensideRookStartSquare(isKingWhite);
                            hasRookMoved = isKingWhite ?
                                game.HasWhiteQueensideRookMoved :
                                game.HasBlackQueensideRookMoved;
                        }

                        else if (isKingsideDirection)
                        {
                            rookStartSquare = GetKingsideRookStartSquare(isKingWhite);
                            hasRookMoved = isKingWhite ?
                                game.HasWhiteKingsideRookMoved :
                                game.HasBlackKingsideRookMoved;
                        }

                        else
                            return MoveSummaryBuilder.DefaultMoveSummary(false, false, Square.NoneSquare);

                        var piece = board[rookStartSquare];
                        if (piece.IsNone())
                            return
                                MoveSummaryBuilder.DefaultMoveSummary(false, false, Square.NoneSquare);

                        var isValid =
                            !hasRookMoved &&
                            piece.color == kingColor &&
                            piece.type == PieceType.Rook;
                        if (isValid)
                            return MoveSummaryBuilder.
                                KingSummary(false, true, isQueensideDirection, isKingsideDirection, Square.NoneSquare);
                    }
                }
            }
            

            return MoveSummaryBuilder.DefaultMoveSummary(false, false, Square.NoneSquare);
        }

        private static Square GetKingStartSquare(bool isKingWhite) =>
            isKingWhite ?
            WhiteCastlingSquares.KingStartSquare :
            BlackCastlingSquares.KingStartSquare;

        private static bool IsQueensideDirection(bool isKingWhite, Square square) =>
            isKingWhite ?
            WhiteCastlingSquares.QueensideKingEndSquare == square :
            BlackCastlingSquares.QueensideKingEndSquare == square;

        private static bool IsKingsideDirection(bool isKingWhite, Square square) =>
            isKingWhite ?
            WhiteCastlingSquares.KingsideKingEndSquare == square :
            BlackCastlingSquares.KingsideKingEndSquare == square;

        private static Square GetQueensideRookStartSquare(bool isWhite) =>
            isWhite ?
            WhiteCastlingSquares.QueensideRookStartSquare :
            BlackCastlingSquares.QueensideRookStartSquare;

        private static Square GetKingsideRookStartSquare(bool isWhite) =>
            isWhite ?
            WhiteCastlingSquares.KingsideRookStartSquare :
            BlackCastlingSquares.KingsideRookStartSquare;
    }

    internal static class PawnMovement
    {
        // TODO: en passant
        public static MoveSummary CanPawnMove(
            ChessGame game,
            Color pawnColor,
            Board board,
            Square from, Square to)
        {
            var isCapturing = false;
            var isMovePossible = false;
            var canPromote = false;
            var enPassantTargetSquare = Square.NoneSquare;
            var isEnPassant = false;

            var canForward = CanForward(pawnColor, board, from, to);
            if (canForward)
            {
                isMovePossible = true;

                var promotionRow = GetPawnPromotionRow(pawnColor);
                if (to.y == promotionRow)
                    canPromote = true;
            }

            var canPush = CanPush(pawnColor, board, from, to);
            if (canPush)
            {
                isMovePossible = true;

                enPassantTargetSquare = new Square(from.x, from.y + GetPawnStep(pawnColor));
            }

            var canCapture = CanCapture(pawnColor, board, from, to);
            if (canCapture)
            {
                isMovePossible = true;
                isCapturing = true;

                var promotionRow = GetPawnPromotionRow(pawnColor);
                if (to.y == promotionRow)
                    canPromote = true;
            }
            var capturedPieceSquare = isCapturing ? to : Square.NoneSquare;
            var canEnPassant = CanEnPassantCapture(
                game, 
                pawnColor,
                //board, 
                from, to);
            if (canEnPassant)
            {
                isMovePossible = true;
                isCapturing = true;
                isEnPassant = true;
                capturedPieceSquare = new Square(
                    game.EnPassantTargetSquare.x,
                    game.EnPassantTargetSquare.y + GetPawnStep(pawnColor));
            }

            return
                MoveSummaryBuilder.PawnMoveSummary(
                    isCapturing, isMovePossible,
                    canPromote, enPassantTargetSquare, isEnPassant, capturedPieceSquare);
        }

        private static bool CanForward(
            Color pawnColor,
            Board board,
            Square from, Square to)
        {
            // white: a1 -> a2; from.y - to.y == 1; from.x == to.x
            // black: a2 -> a1; from.y - to.y == -1; from.x == to.x
            var step = GetPawnStep(pawnColor);
            return
                MoveProperty.DeltaX(from, to) == 0 &&
                MoveProperty.DeltaY(from, to) == step &&
                board[to].IsNone();
        }

        private static bool CanPush(
            Color pawnColor,
            Board board,
            Square from, Square to)
        {
            // white: a2 -> a4; from.y - to.y == 2; from.x == to.x
            // black: a7 -> a5; from.y - to.y == -2; from.x == to.x
            var row = GetPawnStartRow(pawnColor);
            var step = GetPawnStep(pawnColor);
            return
                row == from.y &&
                board[to].IsNone() &&
                board[new Square(from.x, from.y + step)].IsNone() &&
                MoveProperty.DeltaX(from, to) == 0 &&
                MoveProperty.DeltaY(from, to) == 2 * step;
        }

        private static bool CanCapture(
            Color pawnColor,
            Board board,
            Square from, Square to)
        {
            // white: b2 -> a3 || c3; from.y - to.y == 1; |from.x - to.x| == 1
            // black: b7 -> a6 || c6; from.y - to.y == -1; |from.x == to.x| == 1
            var step = GetPawnStep(pawnColor);
            return
                !board[to].IsNone() &&
                MoveProperty.AbsDeltaX(from, to) == 1 &&
                MoveProperty.DeltaY(from, to) == step;
        }

        private static bool CanEnPassantCapture(
            ChessGame game,
            Color pawnColor,
            //Board board,
            Square from, Square to)
        {
            if (game.EnPassantTargetSquare.IsNone())
                return false;

            var step = GetPawnStep(pawnColor);
            return
                to == game.EnPassantTargetSquare &&
                MoveProperty.AbsDeltaX(from, to) == 1 &&
                MoveProperty.DeltaY(from, to) == step;
        }

        private static int GetPawnStep(Color pawnColor) =>
            pawnColor.IsWhite() ? 1 : -1;

        private static int GetPawnStartRow(Color pawnColor) =>
            pawnColor.IsWhite() ? 1 : 6;
        private static int GetPawnPromotionRow(Color pawnColor) =>
            pawnColor.IsWhite() ? 7 : 0;
    }
}
