using ChessEngine.Domain;

namespace ChessEngine.Support
{
    internal static class PieceFactory
    {
        public static Piece WhitePawn() =>
            new Piece(Color.White, PieceType.Pawn);

        public static Piece WhiteKnight() =>
            new Piece(Color.White, PieceType.Knight);

        public static Piece WhiteBishop() =>
            new Piece(Color.White, PieceType.Bishop);

        public static Piece WhiteRook() =>
            new Piece(Color.White, PieceType.Rook);

        public static Piece WhiteQueen() =>
            new Piece(Color.White, PieceType.Queen);

        public static Piece WhiteKing() =>
            new Piece(Color.White, PieceType.King);

        public static Piece BlackPawn() =>
            new Piece(Color.Black, PieceType.Pawn);

        public static Piece BlackKnight() =>
            new Piece(Color.Black, PieceType.Knight);

        public static Piece BlackBishop() =>
            new Piece(Color.Black, PieceType.Bishop);

        public static Piece BlackRook() =>
            new Piece(Color.Black, PieceType.Rook);

        public static Piece BlackQueen() =>
            new Piece(Color.Black, PieceType.Queen);

        public static Piece BlackKing() =>
            new Piece(Color.Black, PieceType.King);

        public static Piece NonePiece() =>
            Piece.NonePiece;
    }
}
