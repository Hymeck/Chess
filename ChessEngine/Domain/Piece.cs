
namespace ChessEngine.Domain
{
    public enum Color
    {
        None,
        White,
        Black
    }

    public static class ColorExtensions
    {
        public static Color GetReversedColor(this Color color) =>
            color == Color.White ? 
                Color.Black : 
                color == Color.Black ?
                    Color.White : Color.None;

        public static bool IsWhite(this Color color) =>
            color == Color.White;
    }

    public enum PieceType
    {
        None,
        Pawn = 'P',
        Knight = 'N',
        Bishop = 'B',
        Rook = 'R',
        Queen = 'Q',
        King = 'K'
    }

    public static class PieceTypeExtensions
    {
        public static char ToChar(this PieceType pieceType) => (char)pieceType;
    }

    public struct Piece
    {
        public static readonly Piece NonePiece = new Piece(Color.None, PieceType.None);

        public readonly Color color;
        public readonly PieceType type;

        public Piece(Color color, PieceType type)
        {
            this.color = color;
            this.type = type;
        }

        public override string ToString() =>
            PieceMethods.GetStringPieceName(this);

        public bool IsNone() =>
            PieceMethods.IsPieceNone(this);
    }

    public static class PieceMethods
    {
        public static char GetCharPieceName(Piece piece) =>
            piece.color.IsWhite() ?
            piece.type.ToChar() : char.ToLower(piece.type.ToChar());

        public static string GetStringPieceName(Piece piece) =>
            GetCharPieceName(piece).ToString();

        public static bool IsPieceNone(Piece piece) =>
            piece.color == Piece.NonePiece.color &&
            piece.type == Piece.NonePiece.type;
    }
}
