using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class BoardCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _cellContent;
    
    [SerializeField]
    private ParticleSystem _starsParticleSystem;

    [Space]
    [Header("Animation properties")]

    [SerializeField]
    private float _scaleAnimationSize = 0.1f;

    [SerializeField]
    private float _bounceAnimationDuration = 0.5f;

    [SerializeField]
    private float _moveAnimationSize = 0.3f;

    [SerializeField]
    private float _moveAnimationDuration = 0.5f;

    private SpriteRenderer _contentRenderer;

    private Sprite _content;
    public Sprite Content { get { return _content; } }
    
    private bool _isClickable = true;

    public BoardCellInteractionEvent OnClicked = new BoardCellInteractionEvent();
    public UnityEvent OnAnimationEnded = new UnityEvent();

    private void Awake()
    {
        _contentRenderer = _cellContent.GetComponent<SpriteRenderer>();

        if (_contentRenderer == null)
        {
            throw new Exception("ContentRenderer is null.");
        }

        if (_starsParticleSystem == null)
        {
            throw new Exception("StarsParticleSystem is null.");
        }
    }

    public void PutContent(Sprite content)
    {
        _content = content;
        _contentRenderer.sprite = content;
    }

    private void OnMouseDown()
    {
        if (!_isClickable)
        {
            return;
        }

        OnClicked?.Invoke(this);
    }

    public void PlayInitializationAnimation()
    {
        StartCoroutine(PlayScaleBounceAnimationCoroutine(
            transform, 
            Ease.InBounce,
            startScale: new Vector3(0, 0, 0)));
    }

    public void PlayCorrectAnimation()
    {
        StartStarsParticles();

        StartCoroutine(PlayScaleBounceAnimationCoroutine(
            _cellContent.transform, 
            Ease.InOutBounce, 
            _cellContent.transform.localScale));
    }

    private void StartStarsParticles()
    {
        ParticleSystem starParticles = 
            Instantiate(_starsParticleSystem, transform.parent.transform);

        starParticles.transform.position += transform.position;

        OnAnimationEnded.AddListener(() => starParticles.Stop());
    }

    public void PlayWrongAnimation()
    {
        StartCoroutine(PlayMoveBounceAnimationCoroutine(
            _cellContent.transform));
    }

    private IEnumerator PlayScaleBounceAnimationCoroutine(Transform targetTransform, 
        Ease ease, Vector3 startScale)
    {
        _isClickable = false;

        PlayScaleBounceAnimation(targetTransform, Ease.InBounce, startScale);
        yield return new WaitForSeconds(_bounceAnimationDuration * 2);

        OnAnimationEnded?.Invoke();

        _isClickable = true;
    }

    private void PlayScaleBounceAnimation(Transform targetTransform, 
        Ease ease, Vector3 startScale)
    {
        Vector3 originalScale = targetTransform.localScale;
        targetTransform.localScale = startScale;
        DOTween.Sequence().Append(
            targetTransform.DOScale(
                new Vector3(
                    originalScale.x + _scaleAnimationSize,
                    originalScale.y + _scaleAnimationSize,
                    originalScale.z + _scaleAnimationSize),
                _bounceAnimationDuration)
                .SetEase(ease))
                .Append(targetTransform.DOScale(
                    originalScale,
                    _bounceAnimationDuration)
                .SetEase(ease));
    }

    private IEnumerator PlayMoveBounceAnimationCoroutine(Transform targetTransform)
    {
        _isClickable = false;

        targetTransform.DOShakePosition(
            _moveAnimationDuration, 
            strength: new Vector3(0, _moveAnimationSize, 0), 
            vibrato: 5, 
            randomness: 1, 
            snapping: false, 
            fadeOut: true);

        yield return new WaitForSeconds(_moveAnimationDuration);

        OnAnimationEnded?.Invoke();

        _isClickable = true;
    }
}
