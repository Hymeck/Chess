namespace ChessEngine.Domain
{
    public record Square
    {
        public static readonly Square NoneSquare = new Square(-1, -1);
        public int X { get; init; }
        public int Y { get; init; }
        public Square(int x, int y) => 
            (X, Y) = (x, y);

        public bool IsOnBoard() => 
            X >= 0 && X <= 7 &&
            Y >= 0 && Y <= 7;

        public string Name =>
            ((char)(X + 'a')).ToString() + (Y + 1).ToString();

        public bool IsNone() =>
            this == NoneSquare;
        
        public static Square FromString(string square)
        {
            var x = square[0] - 'a';
            var y = square[1] - '1';
            return new Square(x, y);
        }
    }
}