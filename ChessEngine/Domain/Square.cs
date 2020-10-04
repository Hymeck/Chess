namespace ChessEngine.Domain
{
    public struct Square
    {
        public static readonly Square NoneSquare = new Square(-1, -1);
        public readonly int x;
        public readonly int y;

        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString() =>
            SquareMethods.GetSquareName(this);

        public bool IsOnBoard() => 
            SquareMethods.IsOnBoard(this);

        public bool IsNone() =>
            SquareMethods.IsSquareNone(this);

        public static bool operator ==(Square s1, Square s2) =>
            SquareMethods.IsEqual(s1, s2);

        public static bool operator !=(Square s1, Square s2) =>
            SquareMethods.IsNotEqual(s1, s2);

    }

    internal static class SquareMethods
    {
        public static Square FromString(string square)
        {
            var x = square[0] - 'a';
            var y = square[1] - '1';
            return new Square(x, y);
        }

        public static bool IsOnBoard(Square s) =>
            s.x >= 0 && s.x <= 7 &&
            s.y >= 0 && s.y <= 7;

        public static string GetSquareName(Square s) =>
            ((char)(s.x + 'a')).ToString() + (s.y + 1).ToString();

        public static bool IsEqual(Square s1, Square s2) =>
            s1.x == s2.x &&
            s1.y == s2.y;

        public static bool IsNotEqual(Square s1, Square s2) =>
            s1.x != s2.x ||
            s1.y != s2.y;

        public static bool IsSquareNone(Square s) =>
            IsEqual(s, Square.NoneSquare);
    }
}