using System.Collections.Generic;
using System.Linq;
using ChessEngine.Movement;
using ChessEngine.Support;

namespace ChessEngine.Domain
{
    public sealed partial class Board
    {
        public readonly Dictionary<Square, Piece> map;

        private Board(Dictionary<Square, Piece> map) =>
            this.map = map;

        public Piece this[Square square] => map[square];
        public Piece this[int x, int y] => this[new Square(x, y)];
    }

    public sealed partial class Board
    {
        public static Board GetStartBoard()
        {
            var map = GetStartMap();
            // var map = GetTestMap();
            return new Board(map);
        }

        internal bool CanCaptureKing(ChessGame game, Square kingSquare)
        {
            var enemyPieces = game.GetPieces(game.ActiveColor.GetReversedColor());
            var canCaptureKing = enemyPieces.Any(
                p =>
                    Rules.CanPieceMove(game, p.Value, this, p.Key, kingSquare).IsMovePossible &&
                    Rules.CanPieceMove(game, p.Value, this, p.Key, kingSquare).IsCapturing);
            
            return canCaptureKing;
        }

        internal bool IsCheckAfterMove(
            ChessGame game,
            Square kingSquare,
            Square from, Square to,
            MoveSummary moveSummary)
        {
            var after = Move(from, to, moveSummary);
            var _kingSquare = kingSquare == from ? to : kingSquare;
            return after.CanCaptureKing(game, _kingSquare);
        }

        internal bool IsCheck(ChessGame game, Square kingSquare) =>
            CanCaptureKing(game, kingSquare);

        private static Dictionary<Square, Piece> GetTestMap()
        {
            var map = new Dictionary<Square, Piece>(64);

            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    map.Add(new Square(x, 7 - y), Piece.NonePiece);

            map[new Square(0, 0)] = PieceFactory.WhiteKing();
            map[new Square(1, 2)] = PieceFactory.BlackRook();
            
            return map;
        }

        private static Dictionary<Square, Piece> GetStartMap()
        {
            var map = new Dictionary<Square, Piece>(64);

            map[new Square(0, 7)] = PieceFactory.BlackRook();
            map[new Square(1, 7)] = PieceFactory.BlackKnight();
            map[new Square(2, 7)] = PieceFactory.BlackBishop();
            map[new Square(3, 7)] = PieceFactory.BlackQueen();
            map[new Square(4, 7)] = PieceFactory.BlackKing();
            map[new Square(5, 7)] = PieceFactory.BlackBishop();
            map[new Square(6, 7)] = PieceFactory.BlackKnight();
            map[new Square(7, 7)] = PieceFactory.BlackRook();
            map[new Square(0, 6)] = PieceFactory.BlackPawn();
            map[new Square(1, 6)] = PieceFactory.BlackPawn();
            map[new Square(2, 6)] = PieceFactory.BlackPawn();
            map[new Square(3, 6)] = PieceFactory.BlackPawn();
            map[new Square(4, 6)] = PieceFactory.BlackPawn();
            map[new Square(5, 6)] = PieceFactory.BlackPawn();
            map[new Square(6, 6)] = PieceFactory.BlackPawn();
            map[new Square(7, 6)] = PieceFactory.BlackPawn();

            for (int y = 2; y < 6; y++)
                for (int x = 0; x < 8; x++)
                    map.Add(new Square(x, 7 - y), Piece.NonePiece);

            map[new Square(0, 1)] = PieceFactory.WhitePawn();
            map[new Square(1, 1)] = PieceFactory.WhitePawn();
            map[new Square(2, 1)] = PieceFactory.WhitePawn();
            map[new Square(3, 1)] = PieceFactory.WhitePawn();
            map[new Square(4, 1)] = PieceFactory.WhitePawn();
            map[new Square(5, 1)] = PieceFactory.WhitePawn();
            map[new Square(6, 1)] = PieceFactory.WhitePawn();
            map[new Square(7, 1)] = PieceFactory.WhitePawn();
            map[new Square(0, 0)] = PieceFactory.WhiteRook();
            map[new Square(1, 0)] = PieceFactory.WhiteKnight();
            map[new Square(2, 0)] = PieceFactory.WhiteBishop();
            map[new Square(3, 0)] = PieceFactory.WhiteQueen();
            map[new Square(4, 0)] = PieceFactory.WhiteKing();
            map[new Square(5, 0)] = PieceFactory.WhiteBishop();
            map[new Square(6, 0)] = PieceFactory.WhiteKnight();
            map[new Square(7, 0)] = PieceFactory.WhiteRook();

            return map;
        }

        internal Board Move(Square from, Square to) =>
            Move(this, from, to);

        internal Board Move((Square, Square) move) =>
            Move(this, move.Item1, move.Item2);

        internal Board Move(Square from, Square to, MoveSummary moveSummary) =>
            Move(this, from, to, moveSummary);

        internal static Board Move(Board currentBoard, Square from, Square to)
        {
            var map = CopyMap(currentBoard);

            map[from] = Piece.NonePiece;
            map[to] = currentBoard[from];

            var nextBoard = new Board(map);
            return nextBoard;
        }

        internal static Board Move(Board currentBoard, Square from, Square to, MoveSummary moveSummary)
        {
            if (!moveSummary.IsMovePossible)
                return currentBoard;
            // TODO: correct mistake with determination of castling direction in KingMovement's CanCastle
            var map = CopyMap(currentBoard);

            map[from] = Piece.NonePiece;
            map[to] = currentBoard[from];

            if (moveSummary.IsEnPassant)
            {
                var stepY = MoveProperty.DeltaY(from, to) < 0 ? 1 : -1;
                var enemyPawnSquare = new Square(to.x, to.y + stepY);
                map[enemyPawnSquare] = Piece.NonePiece;
            }

            if (moveSummary.IsKingCastleQueenside)
            {
                var rookSquare = new Square(0, to.y);

                var rook = map[rookSquare];
                map[rookSquare] = Piece.NonePiece;

                var newRookSquare = new Square(3, to.y);
                map[newRookSquare] = rook;
            }

            if (moveSummary.IsKingCastleKingside)
            {
                var rookSquare = new Square(7, to.y);

                var rook = map[rookSquare];
                map[rookSquare] = Piece.NonePiece;

                var newRookSquare = new Square(5, to.y);
                map[newRookSquare] = rook;
            }

            var nextBoard = new Board(map);
            return nextBoard;
        }

        private static Dictionary<Square, Piece> CopyMap(Board board) =>
            board.map.ToDictionary(
                pair => pair.Key,
                pair => pair.Value);
    }
}
