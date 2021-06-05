using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [SerializeField]
    private GameBoard _gameBoard;

    private List<GameSettings.BoardDifficulty> _levelStates = 
        new List<GameSettings.BoardDifficulty>()
    {
        GameSettings.BoardDifficulty.EASY,
        GameSettings.BoardDifficulty.MEDIUM,
        GameSettings.BoardDifficulty.HARD,
    };

    private int _currentStateIndex = 0;

    public UnityEvent OnLevelCompleted = new UnityEvent();

    private void Start()
    {
        _gameBoard.OnBoardCompleted.AddListener(OnBoardCompleted);

        GenerateNextBoardState();
    }

    private void OnBoardCompleted()
    {
        if (_currentStateIndex >= _levelStates.Count)
        {
            OnLevelCompleted?.Invoke();

            return;
        }

        GenerateNextBoardState();
    }

    private void GenerateNextBoardState()
    {
        if (_currentStateIndex >= _levelStates.Count)
        {
            throw new Exception("Level states don't have any more states.");
        }

        GameSettings.BoardDifficulty nextDifficultyState = 
            _levelStates[_currentStateIndex++];

        GenerateBoard(nextDifficultyState);
    }

    private void GenerateBoard(GameSettings.BoardDifficulty difficulty)
    {
        Vector2Int boardSize = GameSettings.BoardSizesByDifficulty[difficulty];
        GenerateBoardWithSize(boardSize.x, boardSize.y);
    }

    private void GenerateBoardWithSize(int width, int height)
    {
        string[] directories = Directory.GetDirectories(
            Directory.GetCurrentDirectory() + @"\Assets\Resources\Level\Contents\",
            "*", SearchOption.AllDirectories);

        Sprite[] contents = null;
        do
        {
            contents = Resources.LoadAll<Sprite>(Regex.Match(
                directories[UnityEngine.Random.Range(0, directories.Length)],
                @"(?<=Resources\\).*").Value);
        } while (contents.Length < width * height);

        _gameBoard.GenerateCellsWithContents(width, height, contents);
    }
}
