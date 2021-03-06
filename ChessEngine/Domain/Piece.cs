﻿
namespace ChessEngine.Domain
{
    public enum Color
    {
        None = '.',
        White = 'w',
        Black = 'b'
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
        None = '.',
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

    public record Piece
    {
        public static readonly Piece NonePiece = new Piece(Color.None, PieceType.None);

        public Color Color { get; init; }
        public PieceType Type { get; init; }

        public Piece(Color color, PieceType type) =>
            (Color, Type) = (color, type);

        public string Name =>
            Color.IsWhite() ? Type.ToChar().ToString() : char.ToLower(Type.ToChar()).ToString();
        
        public bool IsNone() =>
            this == NonePiece;

        public static Piece FromChar(char piece) =>
            piece switch
            {
                'P' or 'N' or 'B' or 'R' or 'Q' or 'K' => new Piece(Color.White, (PieceType) piece),
                'p' or 'n' or 'b' or 'r' or 'q' or 'k' => new Piece(Color.Black, (PieceType) char.ToUpper(piece)),
                _ => NonePiece
            };
    }
}
