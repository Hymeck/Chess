using System;
using System.Collections.Generic;
using System.Linq;
using ChessEngine.Constants;
using ChessEngine.Domain;

namespace ChessEngine.Support
{
    public class PieceSerializer
    {
        
        // DEFAULT POSITION 
        // rnbqkbnr
        // pppppppp
        // ........
        // ........
        // ........
        // ........
        // PPPPPPPP
        // RNBQKBNR
        public static Dictionary<Square, Piece> GetMap(string map = "rnbqkbnrpppppppp................................PPPPPPPPRNBQKBNR")
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
    }
}