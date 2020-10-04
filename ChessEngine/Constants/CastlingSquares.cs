using ChessEngine.Domain;

namespace ChessEngine.Constants
{
    public static class CastlingSquares
    {
        public static readonly Square WhiteKingStartSquare = new Square(4, 0);
        public static readonly Square BlackKingStartSquare = new Square(4, 7);

        public static readonly Square WhiteQueensideKingEndSquare = new Square(2, 0);
        public static readonly Square BlackQueensideKingEndSquare = new Square(2, 7);

        public static readonly Square WhiteKingsideKingEndSquare = new Square(6, 0);
        public static readonly Square BlackKingsideKingEndSquare = new Square(6, 7);

        public static readonly Square WhiteQueensideRookStartSquare = new Square(0, 0);
        public static readonly Square BlackQueensideRookStartSquare = new Square(0, 7);

        public static readonly Square WhiteKingsideRookStartSquare = new Square(7, 0);
        public static readonly Square BlackKingsideRookStartSquare = new Square(7, 7);

        public static readonly Square WhiteQueensideRookEndSquare = new Square(3, 0);
        public static readonly Square BlackQueensideRookEndSquare = new Square(3, 7);

        public static readonly Square WhiteKingsideRookEndSquare = new Square(5, 0);
        public static readonly Square BlackKingsideRookEndSquare = new Square(5, 7);
    }
}
