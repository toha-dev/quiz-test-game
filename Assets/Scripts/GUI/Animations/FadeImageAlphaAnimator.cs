using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class FadeImageAlphaAnimator : MonoBehaviour
{
    [SerializeField]
    private float _animationDuration = 0.5f;

    public float AnimationDuration { get { return _animationDuration; } }

    [SerializeField]
    private float _minAlpha = 0;

    [SerializeField]
    private float _maxAlpha = 1;
    
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void FadeIn()
    {
        SetImageColorAlpha(_minAlpha);
        StartFadeAnimationToAlpha(_maxAlpha);
    }

    public void FadeOut()
    {
        SetImageColorAlpha(_maxAlpha);
        StartFadeAnimationToAlpha(_minAlpha);
    }

    private void SetImageColorAlpha(float targetAlpha)
    {
        Color targetColor = _image.color;
        targetColor.a = targetAlpha;

        _image.color = targetColor;
    }

    private void StartFadeAnimationToAlpha(float targetAlpha)
    {
        Color endColor = _image.color;
        endColor.a = targetAlpha;

        _image.DOColor(endColor, _animationDuration);
    }
}
