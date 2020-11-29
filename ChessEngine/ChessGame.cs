using ChessEngine.Exceptions;
using ChessEngine.Support;
using System;
using System.Collections.Generic;
using ChessEngine.Constants;
using ChessEngine.Domain;
using ChessEngine.Movement;

namespace ChessEngine
{
    public sealed partial class ChessGame
    {
        public readonly Board Board;
        public readonly Color ActiveColor;
        public readonly Square EnPassantTargetSquare;
        public readonly int MoveCount;

        internal readonly bool HasWhiteKingMoved;
        internal readonly bool HasBlackKingMoved;

        internal readonly bool HasWhiteQueensideRookMoved;
        internal readonly bool HasWhiteKingsideRookMoved;

        internal readonly bool HasBlackQueensideRookMoved;
        internal readonly bool HasBlackKingsideRookMoved;

        private readonly Square WhiteKingSquare;
        private readonly Square BlackKingSquare;

        private readonly Dictionary<Square, Piece> WhitePieces;
        private readonly Dictionary<Square, Piece> BlackPieces;

        #region ChessGame ctor
        public ChessGame() : this(Board.GetStartBoard(), Color.White, Square.NoneSquare,
            hasWhiteKingMoved: false, hasBlackKingMoved: false, 1,
            hasWhiteQueensideRookMoved: false, hasWhiteKingsideRookMoved: false,
            hasBlackQueensideRookMoved: false, hasBlackKingsideRookMoved: false,
            WhiteCastlingSquares.KingStartSquare, BlackCastlingSquares.KingStartSquare) {}
        #endregion ChessGame ctor

        #region ChessGame ctor with parameters
        private ChessGame(
            Board board, Color activeColor,
            Square enPassantTargetSquare,
            bool hasWhiteKingMoved, bool hasBlackKingMoved,
            int moveCount,
            bool hasWhiteQueensideRookMoved, bool hasWhiteKingsideRookMoved,
            bool hasBlackQueensideRookMoved, bool hasBlackKingsideRookMoved,
            Square whiteKingSquare, Square blackKingSquare)
        {
            Board = board;
            ActiveColor = activeColor;
            EnPassantTargetSquare = enPassantTargetSquare;
            HasWhiteKingMoved = hasWhiteKingMoved;
            HasBlackKingMoved = hasBlackKingMoved;
            MoveCount = moveCount;
            HasWhiteQueensideRookMoved = hasWhiteQueensideRookMoved;
            HasWhiteKingsideRookMoved = hasWhiteKingsideRookMoved;

            HasBlackQueensideRookMoved = hasBlackQueensideRookMoved;
            HasBlackKingsideRookMoved = hasBlackKingsideRookMoved;

            WhiteKingSquare = whiteKingSquare;
            BlackKingSquare = blackKingSquare;

            var (whitePieces, blackPieces) = GetPieces(Board);
            WhitePieces = whitePieces;
            BlackPieces = blackPieces;
        }
        #endregion ChessGame ctor with parameters

        #region ChessGame ctor with fen parameter
        public ChessGame(string fen)
        {
            var parts = fen.Split();
            PieceSerializer.ParseBoard(parts[0], out WhiteKingSquare, out BlackKingSquare, out Board);
            ActiveColor = PieceSerializer.ToColor(parts[1]);
            var (hasWhiteKingMoved, hasBlackKingMoved, hasWhiteQueensideRookMoved, hasWhiteKingsideRookMoved, hasBlackQueensideRookMoved, hasBlackKingsideRookMoved) = PieceSerializer.ParseCastlings(parts[2]);
            HasWhiteKingMoved = hasWhiteKingMoved;
            HasBlackKingMoved = hasBlackKingMoved;
            HasWhiteQueensideRookMoved = hasWhiteQueensideRookMoved;
            HasWhiteKingsideRookMoved = hasWhiteKingsideRookMoved;
            HasBlackQueensideRookMoved = hasBlackQueensideRookMoved;
            HasBlackKingsideRookMoved = hasBlackKingsideRookMoved;
            EnPassantTargetSquare = PieceSerializer.ToSquare(parts[3]);
            MoveCount = int.Parse(parts[5]);
            
            var (whitePieces, blackPieces) = GetPieces(Board);
            WhitePieces = whitePieces;
            BlackPieces = blackPieces;
        }
        #endregion ChessGame ctor with fen parameter
        
        public bool IsCheck =>
            Board.IsCheck(this, GetActualKingSquare());

        public bool IsCheckmate
        {
            get
            {
                if (!IsCheck)
                    return false;

                var possibleMoveCount = 0;
                foreach (var m in YieldPossibleMoves())
                    possibleMoveCount++;

                return possibleMoveCount == 0;
            }
        }

        public bool IsStalemate
        {
            get
            {
                if (IsCheck)
                    return false;

                var possibleMoveCount = 0;
                foreach (var m in YieldPossibleMoves())
                    possibleMoveCount++;

                return possibleMoveCount == 0;
            }
        }

        public override string ToString() => 
            PieceSerializer.ToFen(this);
    }

    public sealed partial class ChessGame
    {
        private (
            Dictionary<Square, Piece> whitePieces,
            Dictionary<Square, Piece> blackPieces) GetPieces(Board board)
        {
            var whitePieces = new Dictionary<Square, Piece>(16);
            var blackPieces = new Dictionary<Square, Piece>(16);

            foreach (var p in board.map)
            {
                if (!p.Value.IsNone())
                {
                    if (p.Value.Color.IsWhite())
                        whitePieces.Add(p.Key, p.Value);
                    else
                        blackPieces.Add(p.Key, p.Value);
                }
            }

            return (whitePieces, blackPieces);
        }

        private static (
            bool hasWhiteKingMoved,
            bool hasBlackKingMoved,
            bool hasWhiteQueensideRookMoved,
            bool hasWhiteKingsideRookMoved,
            bool hasBlackQueensideRookMoved,
            bool hasBlackKingsideRookMoved) GetKingAndRookInfo(ChessGame game, MoveSummary moveSummary)
        {
            var isActiveColorWhite = game.ActiveColor.IsWhite();
            var hasWhiteKingMoved = false;
            if (game.HasWhiteKingMoved)
                hasWhiteKingMoved = true;
            else
            {
                if (isActiveColorWhite)
                    hasWhiteKingMoved = moveSummary.IsKingMoving;
            }

            var hasBlackKingMoved = false;
            if (game.HasBlackKingMoved)
                hasWhiteKingMoved = true;
            else
            {
                if (!isActiveColorWhite)
                    hasBlackKingMoved = moveSummary.IsKingMoving;
            }


            var hasWhiteQueensideRookMoved = false;
            if (game.HasWhiteQueensideRookMoved)
                hasWhiteQueensideRookMoved = true;
            else
            {
                if (isActiveColorWhite)
                    hasWhiteQueensideRookMoved = moveSummary.IsQueensideRookMoving;
            }

            var hasWhiteKingsideRookMoved = false;
            if (game.HasWhiteKingsideRookMoved)
                hasWhiteKingsideRookMoved = true;
            else
            {
                if (isActiveColorWhite)
                    hasWhiteKingsideRookMoved = moveSummary.IsKingsideRookMoving;
            }


            var hasBlackQueensideRookMoved = false;
            if (game.HasBlackQueensideRookMoved)
                hasBlackQueensideRookMoved = true;
            else
            {
                if (!isActiveColorWhite)
                    hasBlackQueensideRookMoved = moveSummary.IsQueensideRookMoving;
            }

            var hasBlackKingsideRookMoved = false;
            if (game.HasBlackKingsideRookMoved)
                hasBlackKingsideRookMoved = true;
            else
            {
                if (!isActiveColorWhite)
                    hasBlackKingsideRookMoved = moveSummary.IsKingsideRookMoving;
            }

            return (
                hasWhiteKingMoved,
                hasBlackKingMoved,
                hasWhiteQueensideRookMoved,
                hasWhiteKingsideRookMoved,
                hasBlackQueensideRookMoved,
                hasBlackKingsideRookMoved);
        }
        
        /// <summary>
        /// Makes move
        /// </summary>
        /// <param name="moveSquares">tuple of start and end squares</param>
        /// <exception cref="PieceNotFoundException">Board does not contain piece on start square</exception>
        /// <exception cref="SquareException">Start or end square does not lie within the board</exception>
        /// <exception cref="EqualSquareException">Start square equals to end square</exception>
        /// <exception cref="ColorException">Moving piece matches by color with piece staying on end square or piece does not match with turn color</exception>
        /// <exception cref="PieceMoveException">Piece move is not made by the rule</exception>
        /// <exception cref="CheckException">The king is still in check after move</exception>
        /// <returns>game</returns>
        public ChessGame Move((Square from, Square to) moveSquares) =>
            Move(moveSquares.from, moveSquares.to);

        internal Dictionary<Square, Piece> GetPieces(Color color) =>
            color.IsWhite() ? WhitePieces : BlackPieces;

        private Square GetActualKingSquare() =>
            ActiveColor.IsWhite() ? WhiteKingSquare : BlackKingSquare;

        private bool IsCheckAfterMove(Square from, Square to, MoveSummary moveSummary) =>
            Board.IsCheckAfterMove(this, GetActualKingSquare(), from, to, moveSummary);

        /// <summary>
        /// Makes move
        /// </summary>
        /// <param name="from">start square</param>
        /// <param name="to">end square</param>
        /// <exception cref="PieceNotFoundException">Board does not contain piece on start square</exception>
        /// <exception cref="SquareException">Start or end square does not lie within the board</exception>
        /// <exception cref="EqualSquareException">Start square equals to end square</exception>
        /// <exception cref="ColorException">Moving piece matches by color with piece staying on end square or piece does not match with turn color</exception>
        /// <exception cref="PieceMoveException">Piece move is not made by the rule</exception>
        /// <exception cref="CheckException">The king is still in check after move</exception>
        public ChessGame Move(Square from, Square to)
        {
            #region checking with exceptions
            var piece = Board[from];
            if (piece.IsNone())
                throw new PieceNotFoundException(from);
                // return this;

            var areSquareOnBoard = Checker.AreSquaresOnBoard(from, to);
            if (!areSquareOnBoard)
                throw new SquareException();
                // return this;

            var areSquaresEqual = Checker.AreSquaresEqual(from, to);
            if (areSquaresEqual)
                throw new EqualSquareException();
                // return this;

            var isValidColor = Checker.IsValidColors(ActiveColor, piece, Board.map[to]);
            if (!isValidColor)
                throw new ColorException(ActiveColor, piece.Color, Board.map[to].Color);
                // return this;

            var moveSummary = Rules.CanPieceMove(this, piece, Board, from, to);
            if (!moveSummary.IsMovePossible)
                throw new PieceMoveException(piece, from, to);
                // return this;

            var isCheckAfterMove = IsCheckAfterMove(from, to, moveSummary);
            if (isCheckAfterMove)
                throw new CheckException(piece, from, to);
                // return this;
            #endregion checking with exceptions
            // pawn:
            // promotion -> ???
            #region setting game variables
            var isActiveColorWhite = ActiveColor.IsWhite();

            var whiteKingSquare = isActiveColorWhite && moveSummary.IsKingMoving ?
                to : WhiteKingSquare;
            var blackKingSquare = !isActiveColorWhite && moveSummary.IsKingMoving ?
                to : BlackKingSquare;

            var nextBoard = Board.Move(from, to, moveSummary);

            var nextMoveCount = !isActiveColorWhite ? MoveCount + 1 : MoveCount;
            var enPassantTargetSquare = moveSummary.EnPassantTargetSquare;

            var (
                hasWhiteKingMoved,
                hasBlackKingMoved,
                hasWhiteQueensideRookMoved,
                hasWhiteKingsideRookMoved,
                hasBlackQueensideRookMoved,
                hasBlackKingsideRookMoved) = GetKingAndRookInfo(this, moveSummary);

            var nextColor = ActiveColor.GetReversedColor();
            #endregion setting game variables

            return new ChessGame(
                nextBoard,
                nextColor,
                enPassantTargetSquare,
                hasWhiteKingMoved,
                hasBlackKingMoved,
                nextMoveCount,
                hasWhiteQueensideRookMoved,
                hasWhiteKingsideRookMoved,
                hasBlackQueensideRookMoved,
                hasBlackKingsideRookMoved,
                whiteKingSquare,
                blackKingSquare);
        }

        public IEnumerable<string> YieldPieces()
        {
            var pieces = GetPieces(ActiveColor);
            foreach (var p in pieces)
                yield return p.Value.ToString();
        }

        internal IEnumerable<(Square from, Square to)> YieldPossibleMoves()
        {
            var pieces = GetPieces(ActiveColor);
            var kingSquare = GetActualKingSquare();
            foreach (var pieceSquare in pieces)
            {
                foreach (var boardSquare in Board.map)
                {
                    var moveSummary = Rules.CanPieceMove(
                        this,
                        pieceSquare.Value,
                        Board,
                        pieceSquare.Key,
                        boardSquare.Key);

                    var isCheckAfterMove = Board.IsCheckAfterMove(
                        this,
                        kingSquare, pieceSquare.Key,
                        boardSquare.Key,
                        moveSummary);

                    if (moveSummary.IsMovePossible && !isCheckAfterMove)
                        yield return (pieceSquare.Key, boardSquare.Key);
                }
            }
        }
        public IEnumerable<string> YieldMoves()
        {
            foreach (var (from, to) in YieldPossibleMoves())
                yield return from.Name + to.Name;
        }
    }
}
