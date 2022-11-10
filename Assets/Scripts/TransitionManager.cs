using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(WaitToSwitchScene());
    }

    public IEnumerator WaitToSwitchScene()
    {
        yield return new WaitForSeconds(9);
        SceneManager.LoadScene(3);
    }
}
