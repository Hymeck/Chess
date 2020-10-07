using ChessEngine.Domain;

namespace ChessEngine.Constants
{
    internal class WhiteCastlingSquares
    {
        public static readonly Square KingStartSquare = new Square(4, 0);
        public static readonly Square QueensideKingEndSquare = new Square(2, 0);
        public static readonly Square KingsideKingEndSquare = new Square(6, 0);

        public static readonly Square QueensideRookStartSquare = new Square(0, 0);
        public static readonly Square KingsideRookStartSquare = new Square(7, 0);
    }
}