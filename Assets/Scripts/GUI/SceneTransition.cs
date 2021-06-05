using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG;

[RequireComponent(typeof(FadeImageAlphaAnimator))]
public class SceneTransition : MonoBehaviour
{
    private FadeImageAlphaAnimator _fadeController;

    public UnityEvent OnFadeInCompleted = new UnityEvent();

    private void Awake()
    {
        _fadeController = GetComponent<FadeImageAlphaAnimator>();
    }

    public void FadeInSceneBeforeReload()
    {
        StartCoroutine(FadeInScene());
    }

    private IEnumerator FadeInScene()
    {
        _fadeController.FadeIn();

        yield return new WaitForSeconds(_fadeController.AnimationDuration);
        OnFadeInCompleted?.Invoke();
    }
}
