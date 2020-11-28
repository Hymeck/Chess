using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessEngine.Constants;
using ChessEngine.Domain;

namespace ChessEngine.Support
{
    internal static class PieceSerializer
    {
        private static readonly string map = "rnbqkbnrpppppppp................................PPPPPPPPRNBQKBNR";
        private static readonly string eight = "11111111";
        // DEFAULT POSITION 
        // rnbqkbnr
        // pppppppp
        // ........
        // ........
        // ........
        // ........
        // PPPPPPPP
        // RNBQKBNR
        public static Dictionary<Square, Piece> FromFen()
        {
            var resultMap = new Dictionary<Square, Piece>(64);

            for (var i = 0; i < map.Length; i++)
            {
                var x = i % 8;
                var y = 7 - (i / 8);
                resultMap[new Square(x, y)] = Piece.FromChar(map[i]);
            }

            return resultMap;
        }

        public static void ParseBoard(string fenPieces, out Square whiteKingSquare, out Square blackKingSquare, out Board board)
        {
            var data = new StringBuilder(fenPieces);
            for (var j = 8; j >= 2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
            data = data.Replace("1", ".");
            var rows = data.ToString().Split('/');

            var map = new Dictionary<Square, Piece>(64);
            for (var y = 7; y >= 0; y--)
            for (var x = 0; x < 8; x++)
                map[new Square(x, y)] = Piece.FromChar(rows[7 - y][x]);
            whiteKingSquare = map.FirstOrDefault(x => x.Value == PieceFactory.WhiteKing()).Key;
            blackKingSquare = map.FirstOrDefault(x => x.Value == PieceFactory.BlackKing()).Key;
            board = new Board(map);
        }

        public static Color ToColor(string color) =>
            color == "w" ? Color.White : Color.Black;

        public static Square ToSquare(string square) =>
            Square.FromString(square);

        public static (
            bool hasWhiteKingMoved,
            bool hasBlackKingMoved,
            bool hasWhiteQueensideRookMoved,
            bool hasWhiteKingsideRookMoved,
            bool hasBlackQueensideRookMoved,
            bool hasBlackKingsideRookMoved) 
            ParseCastlings(string castlings)
        {
            bool hasWhiteKingMoved = default,
                hasBlackKingMoved = default,
                hasWhiteQueensideRookMoved = default,
                hasWhiteKingsideRookMoved = default,
                hasBlackQueensideRookMoved = default,
                hasBlackKingsideRookMoved = default;
            
            if (castlings == "-")
            {
                hasWhiteKingMoved = hasBlackKingMoved = true;
            }

            if (!castlings.Contains("KQ"))
            {
                if (castlings.Contains('K'))
                    hasWhiteQueensideRookMoved = true;
                else if (castlings.Contains('Q'))
                    hasWhiteKingsideRookMoved = true;
                else
                    hasWhiteKingMoved = true;
            }
            
            if (!castlings.Contains("kq"))
            {
                if (castlings.Contains('k'))
                    hasBlackQueensideRookMoved = true;
                else if (castlings.Contains('q'))
                    hasBlackKingsideRookMoved = true;
                else
                    hasBlackKingMoved = true;
            }
            
            
            return (
                hasWhiteKingMoved,
                hasBlackKingMoved,
                hasWhiteQueensideRookMoved,
                hasWhiteKingsideRookMoved,
                hasBlackQueensideRookMoved,
                hasBlackKingsideRookMoved);
        }
        
        public static string ToFen(ChessGame game)
        {
            var piecePosition = ToPiecePositions(game.Board);
            var activeColor = (char) game.ActiveColor;
            var castlings = GetCastlings(game);
            var square = game.EnPassantTargetSquare.IsNone() ? "-" : game.EnPassantTargetSquare.Name;
            char ply = '0';
            var moveCount = game.MoveCount.ToString();
            return $"{piecePosition} {activeColor} {castlings} {square} {ply} {moveCount}";
        }

        public static string GetCastlings(ChessGame game)
        {
            if (game.HasWhiteKingMoved && game.HasBlackKingMoved)
                return "-";
            var sb = new StringBuilder(4);
            if (!game.HasWhiteKingMoved)
            {
                if (!game.HasWhiteKingsideRookMoved)
                    sb.Append('K');
                if (!game.HasWhiteQueensideRookMoved)
                    sb.Append('Q');
            }
            
            if (!game.HasBlackKingMoved)
            {
                if (!game.HasBlackKingsideRookMoved)
                    sb.Append('k');
                if (!game.HasBlackQueensideRookMoved)
                    sb.Append('q');
            }

            return sb.ToString();
        }

        private static string ToPiecePositions(Board board)
        {
            var sb = new StringBuilder();
            for (var y = 7; y >= 0; y--)
            {
                for (var x = 0; x < 8; x++) 
                    sb.Append(board[x, y].IsNone() ? '1' : board[x, y].Name);
                if (y > 0)
                    sb.Append('/');
            }
            
            for (int j = 8; j >= 2; j--)
                sb.Replace(eight.Substring(0, j), j.ToString());
            return sb.ToString();
        }
    }
}