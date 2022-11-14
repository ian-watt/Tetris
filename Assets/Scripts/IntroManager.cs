using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    //simple intro script to help with fading in and out to main menu

    private void Awake()
    {
       StartCoroutine(WaitToSwitchScene());
    }

    public IEnumerator WaitToSwitchScene()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(1);
    }
}
