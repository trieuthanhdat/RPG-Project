using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    [SerializeField] float timeToDisable = 3f;
    private void Update()
    {
        if(gameObject.activeInHierarchy)
        {
            StartCoroutine(SetActiveObject());
        }

    }
    private IEnumerator SetActiveObject()
    {
        yield return new WaitForSeconds(timeToDisable);
        gameObject.SetActive(false);
    }
}
