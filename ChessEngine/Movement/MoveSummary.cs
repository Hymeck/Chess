using ChessEngine.Domain;

namespace ChessEngine.Movement
{
    internal record MoveSummary
    {
        // common
        public readonly bool IsMovePossible;
        public readonly bool IsCapturing;
        public readonly Square CapturedPieceSquare;

        // pawn
        public readonly Square EnPassantTargetSquare;
        public readonly bool IsPromoting;
        public readonly bool IsPawnMoving;
        public readonly bool IsEnPassant;

        // rook
        public readonly bool IsQueensideRookMoving;
        public readonly bool IsKingsideRookMoving;

        // king
        public readonly bool IsKingMoving;
        public readonly bool IsKingCastleQueenside;
        public readonly bool IsKingCastleKingside;

        public MoveSummary(
            bool isCapturing, 
            bool isMovePossible, 
            Square enPassantTargetSquare,
            Square capturedPieceSquare,
            bool isPromoting, 
            bool isPawnMoving, 
            bool isQueensideRookMoving, 
            bool isKingsideRookMoving, 
            bool isKingMoving,
            bool isKingCastleQueenside = false,
            bool isKingCastleKingside = false,
            bool isEnPassant = false)
        {
            IsCapturing = isCapturing;
            IsMovePossible = isMovePossible;
            EnPassantTargetSquare = enPassantTargetSquare;
            IsPromoting = isPromoting;
            IsPawnMoving = isPawnMoving;
            IsQueensideRookMoving = isQueensideRookMoving;
            IsKingsideRookMoving = isKingsideRookMoving;
            IsKingMoving = isKingMoving;
            IsKingCastleQueenside = isKingCastleQueenside;
            IsKingCastleKingside = isKingCastleKingside;
            IsEnPassant = isEnPassant;
            CapturedPieceSquare = capturedPieceSquare;
        }
    }

    internal static class MoveSummaryBuilder
    {
        public static MoveSummary NoneMoveSummary() =>
            new MoveSummary(
                false,
                false,
                Square.NoneSquare,
                Square.NoneSquare,
                isPromoting: false,
                isPawnMoving: false,
                isQueensideRookMoving: false,
                isKingsideRookMoving: false,
                isKingMoving: false);
        public static MoveSummary DefaultMoveSummary(bool isCapturing, bool isMovePossible, Square capturedPieceSquare) =>
            new MoveSummary(
                isCapturing,
                isMovePossible,
                Square.NoneSquare,
                capturedPieceSquare,
                isPromoting: false,
                isPawnMoving: false,
                isQueensideRookMoving: false,
                isKingsideRookMoving: false,
                isKingMoving: false);

        public static MoveSummary PawnMoveSummary(
            bool isCapturing,
            bool isMovePossible,
            bool isPromoting,
            Square enPassantTargetSquare,
            bool isEnPassant,
            Square capturedPieceSquare) =>
            new MoveSummary(
                isCapturing,
                isMovePossible,
                enPassantTargetSquare,
                capturedPieceSquare,
                isPromoting,
                isPawnMoving: true,
                isQueensideRookMoving: false,
                isKingsideRookMoving: false,
                isKingMoving: false,
                isEnPassant: isEnPassant);

        public static MoveSummary RookSummary(
            bool isCapturing,
            bool isMovePossible,
            bool isQueensideRookMoving,
            bool isKingsideRookMoving,
            Square capturedPieceSquare) =>
            new MoveSummary(
                isCapturing,
                isMovePossible,
                Square.NoneSquare,
                capturedPieceSquare,
                isPromoting: false,
                isPawnMoving: false,
                isQueensideRookMoving: isQueensideRookMoving,
                isKingsideRookMoving: isKingsideRookMoving,
                isKingMoving: false);

        public static MoveSummary KingSummary(
            bool isCapturing, 
            bool isMovePossible,
            bool isKingCastleQueenside,
            bool isKingCastleKingside,
            Square capturedPieceSquare) =>
            new MoveSummary(
                isCapturing,
                isMovePossible,
                Square.NoneSquare,
                capturedPieceSquare,
                isPromoting: false,
                isPawnMoving: false,
                isQueensideRookMoving: false,
                isKingsideRookMoving: false,
                isKingMoving: true,
                isKingCastleQueenside: isKingCastleQueenside,
                isKingCastleKingside: isKingCastleKingside);
    }
}
