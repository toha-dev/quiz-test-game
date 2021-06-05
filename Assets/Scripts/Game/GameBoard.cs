using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject _background;

    [SerializeField]
    private GameObject _gameCellsParent;

    [Space]
    [Header("Board parameters")]

    [SerializeField]
    private float _boardEdgeWidth = 0.1f;

    private GameObject _cellPrefab;

    private int _width;
    private int _height;

    private bool _isCompleted = false;
    private bool _isFirstInitialization = true;

    private List<BoardCell> _boardCells = new List<BoardCell>();
    private Sprite _targetContent;

    private UniqueRandomPicker<List<Sprite>, Sprite> _targetUniqueRandomPicker =
        new UniqueRandomPicker<List<Sprite>, Sprite>();

    public string TargetText { get { return _targetContent.name; } }

    public UnityEvent OnBoardCompleted = new UnityEvent();
    public UnityEvent OnTargetUpdated = new UnityEvent();

    private void Awake()
    {
        _cellPrefab = Resources.Load<GameObject>(@"Prefabs\Level\Board Cell");

        if (_cellPrefab == null)
        {
            throw new Exception("Cell prefab doesn't loaded.");
        }
    }

    public void GenerateCellsWithContents(int width, int height, Sprite[] contents)
    {
        if (_boardCells.Count != 0)
        {
            DestroyOldBoardCells();
        }

        _isCompleted = false;
        _width = width;
        _height = height;

        ResizeBackground();

        UniqueRandomPicker<List<Sprite>, Sprite> randomContentPicker = 
            new UniqueRandomPicker<List<Sprite>, Sprite>(
                new List<Sprite>(contents));

        List<Sprite> currentContents = new List<Sprite>();

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                GameObject instantiatedCell = Instantiate(_cellPrefab, 
                    _gameCellsParent.transform);

                instantiatedCell.transform.position = CalculateCellPosition(x, y);

                BoardCell boardCell = instantiatedCell.GetComponent<BoardCell>();
                boardCell.PutContent(randomContentPicker.GetNextUniqueRandomElement());
                boardCell.OnClicked.AddListener(OnBoardCellClicked);

                _boardCells.Add(boardCell);
                currentContents.Add(boardCell.Content);
            }
        }

        _targetUniqueRandomPicker.SetContainer(currentContents);
        _targetContent = _targetUniqueRandomPicker.GetNextUniqueRandomElement();

        if (_isFirstInitialization)
        {
            foreach (var boardCell in _boardCells)
            {
                boardCell.PlayInitializationAnimation();
            }

            _isFirstInitialization = false;
        }

        OnTargetUpdated?.Invoke();
    }

    private void ResizeBackground()
    {
        float backgroundWidth = _width * _cellPrefab.transform.localScale.x
            + _boardEdgeWidth;
        float backgroundHeight = _height * _cellPrefab.transform.localScale.y
            + _boardEdgeWidth;

        _background.transform.localScale = new Vector3(
            backgroundWidth,
            backgroundHeight,
            _background.transform.localScale.z);
    }

    private Vector3 CalculateCellPosition(int x, int y)
    {
        Vector3 boardCenter = transform.position;
        Vector3 topLeftCellPosition = new Vector3(
            boardCenter.x - (_width - 1) / 2f * _cellPrefab.transform.localScale.x,
            boardCenter.y + (_height - 1) / 2f * _cellPrefab.transform.localScale.y,
            boardCenter.z);

        Vector3 result = new Vector3(
            topLeftCellPosition.x + x * _cellPrefab.transform.localScale.x,
            topLeftCellPosition.y - y * _cellPrefab.transform.localScale.y,
            boardCenter.z);

        return result;
    }

    private void DestroyOldBoardCells()
    {
        foreach (var boardCell in _boardCells)
        {
            Destroy(boardCell.gameObject);
        }

        _boardCells.Clear();
    }

    private void OnBoardCellClicked(BoardCell initiator)
    {
        if (_isCompleted)
        {
            return;
        }

        if (initiator.Content == _targetContent)
        {
            initiator.OnAnimationEnded.AddListener(() => OnBoardCompleted?.Invoke());
            initiator.PlayCorrectAnimation();

            _isCompleted = true;
        }
        else
        {
            initiator.PlayWrongAnimation();
        }
    }
}
