using ChessEngine.Domain;

namespace ChessEngine.Movement
{
    internal static class Rules
    {
        public static MoveSummary CanPieceMove(
            ChessGame game,
            Piece piece, Board board,
            Square from, Square to) =>
            piece.Type switch
            {
                PieceType.Pawn => PawnMovement.CanPawnMove(game, piece.Color, board, @from, to),
                PieceType.Knight => KnightMovement.CanKnightMove(board, @from, to),
                PieceType.Bishop => BishopMovement.CanBishopMove(board, @from, to),
                PieceType.Rook => RookMovement.CanRookMove(board, @from, to),
                PieceType.Queen => QueenMovement.CanQueenMove(board, @from, to),
                PieceType.King => KingMovement.CanKingMove(game, piece.Color, board, @from, to),
                _ => MoveSummaryBuilder.NoneMoveSummary()
            };
    }
}
