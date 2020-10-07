using ChessEngine.Domain;

namespace ChessEngine.Constants
{
    internal static class BlackCastlingSquares
    {
        public static readonly Square KingStartSquare = new Square(4, 7);
        public static readonly Square QueensideKingEndSquare = new Square(2, 7);
        public static readonly Square KingsideKingEndSquare = new Square(6, 7);
        
        public static readonly Square QueensideRookStartSquare = new Square(0, 7);
        public static readonly Square KingsideRookStartSquare = new Square(7, 7);
    }
}
