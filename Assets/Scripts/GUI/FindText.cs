using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Text))]
public class FindText : MonoBehaviour
{
    [SerializeField]
    private GameBoard _gameBoard;

    [Space]
    [Header("Animation parameters")]

    [SerializeField]
    private float _animationDuration = 1.5f;

    private Text _text;
    private string _preambule;

    private void Awake()
    {
        _text = GetComponent<Text>();
        _preambule = _text.text;

        if (_gameBoard == null)
        {
            throw new Exception("GameBoard is null.");
        }

        _gameBoard.OnTargetUpdated.AddListener(OnTargetUpdated);

        PlayAwakeAnimation();
    }

    private void PlayAwakeAnimation()
    {
        Color endColor = _text.color;
        endColor.a = 1;

        _text.DOColor(endColor, _animationDuration);
    }

    private void OnTargetUpdated()
    {
        _text.text = _preambule + " " + _gameBoard.TargetText.ToUpper();
    }
}
