using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FadeImageAlphaAnimator))]
public class FinalScreen : MonoBehaviour
{
    [SerializeField]
    private Level _level;

    private FadeImageAlphaAnimator _fadeController;

    private void Awake()
    {
        if (_level == null)
        {
            throw new System.Exception("Level is null.");
        }

        _fadeController = GetComponent<FadeImageAlphaAnimator>();

        _level.OnLevelCompleted.AddListener(OnLevelCompleted);

        gameObject.SetActive(false);
    }
    
    private void OnLevelCompleted()
    {
        gameObject.SetActive(true);
        _fadeController.FadeIn();
    }
}
