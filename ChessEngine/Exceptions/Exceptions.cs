using System;
using ChessEngine.Domain;

namespace ChessEngine.Exceptions
{
    public abstract class ChessGameException : Exception
    {
        public ChessGameException(string message) : base(message) {}
    }
    public sealed class PieceNotFoundException : ChessGameException
    {
        public PieceNotFoundException(Square square) : 
            base($"Piece on {square} doesn't exist") { }
    }

    public sealed class PieceMoveException : ChessGameException
    {
        public PieceMoveException(Piece piece, Square from, Square to) : 
            base ($"{piece} can't make '{from.ToString() + to.ToString()}' move") { }
    }

    public sealed class ColorException : ChessGameException
    {
        public ColorException(Color activeColor, Color pieceColor, Color enemyColor) : 
            base($"Piece color ({pieceColor}) doesn't correspond with move color ({activeColor}) or matches with color of piece ({enemyColor}) staying on 'to' square") { }
    }

    public sealed class SquareException : ChessGameException
    {
        public SquareException() : 
            base("Specified squares are out of the board") { }
    }

    public sealed class EqualSquareException : ChessGameException
    {
        public EqualSquareException() : 
            base("Specified squares are the same") { }
    }

    public sealed class CheckException : ChessGameException
    {
        public CheckException(Piece piece, Square from, Square to) :
            base($"Piece ({piece}) can't make specified move ({from.ToString() + to.ToString()}) due to king check") { }
    }
}
