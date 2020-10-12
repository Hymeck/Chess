using System;
using ChessEngine.Domain;

namespace ChessEngine.Exceptions
{
    public sealed class PieceNotFoundException : Exception
    {
        public PieceNotFoundException(Square square) : 
            base($"Piece on {square} doesn't exist") { }
    }

    public sealed class PieceMoveException : Exception
    {
        public PieceMoveException(Piece piece, Square from, Square to) : 
            base ($"{piece} can't make '{from.ToString() + to.ToString()}' move") { }
    }

    public sealed class ColorException : Exception
    {
        public ColorException(Color activeColor, Color pieceColor, Color enemyColor) : 
            base($"Piece color ({pieceColor}) doesn't correspond with move color ({activeColor}) or matches with color of piece ({enemyColor}) staying on 'to' square") { }
    }

    public sealed class SquareException : Exception
    {
        public SquareException() : 
            base("Specified squares are out of the board") { }
    }

    public sealed class EqualSquareException : Exception
    {
        public EqualSquareException() : 
            base("Specified squares are the same") { }
    }

    public sealed class CheckException : Exception
    {
        public CheckException(Piece piece, Square from, Square to) :
            base($"Piece ({piece}) can't make specified move ({from.ToString() + to.ToString()}) due to king check") { }
    }
}
