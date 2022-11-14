using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{

    //simple transitional script to manage the transition scene

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
