using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    public enum BoardDifficulty
    {
        EASY,
        MEDIUM,
        HARD,
    }

    public static Dictionary<BoardDifficulty, Vector2Int> BoardSizesByDifficulty = 
        new Dictionary<BoardDifficulty, Vector2Int>()
    {
        { BoardDifficulty.EASY, new Vector2Int(3, 1) },
        { BoardDifficulty.MEDIUM, new Vector2Int(3, 2) },
        { BoardDifficulty.HARD, new Vector2Int(3, 3) },
    };
}
