using ChessEngine.Domain;

namespace ChessEngine.Movement
{
    internal static class Rules
    {
        public static MoveSummary CanPieceMove(ChessGame game, Piece piece, Board board, Square from, Square to)
        {
            var moveSummary = MoveSummaryBuilder.NoneMoveSummary();
            
            var type = piece.type;
            switch (type)
            {
                case PieceType.Pawn:
                    moveSummary = PawnMovement.CanPawnMove(game, piece.color, board, from, to);
                    break;

                case PieceType.Knight:
                    moveSummary = KnightMovement.CanKnightMove(board, from, to);
                    break;

                case PieceType.Bishop:
                    moveSummary = BishopMovement.CanBishopMove(board, from, to);
                    break;

                case PieceType.Rook:
                    moveSummary = RookMovement.CanRookMove(board, from, to);
                    break;

                case PieceType.Queen:
                    moveSummary = QueenMovement.CanQueenMove(board, from, to);
                    break;

                case PieceType.King:
                    moveSummary = KingMovement.CanKingMove(game, piece.color, board, from, to);
                    break;
            }

            return moveSummary;
        }
    }
}
