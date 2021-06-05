using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RestartButtonController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private SceneTransition _sceneTransitionController;

    private void Awake()
    {
        if (_sceneTransitionController == null)
        {
            throw new Exception("SceneTransitionController is null.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Scene scene = SceneManager.GetActiveScene();

        _sceneTransitionController.OnFadeInCompleted.AddListener(
            () => { SceneManager.LoadScene(scene.name); });

        _sceneTransitionController.FadeInSceneBeforeReload();
    }
}
