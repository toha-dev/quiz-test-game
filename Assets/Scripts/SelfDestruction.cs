using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    [SerializeField]
    private float _timeToDestruct;

    private void Start()
    {
        StartCoroutine(DestructItselfWithDelay());
    }

    private IEnumerator DestructItselfWithDelay()
    {
        yield return new WaitForSeconds(_timeToDestruct);

        Destroy(gameObject);
    }
}
