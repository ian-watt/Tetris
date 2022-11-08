using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject highScoreMenu;
    public static MainMenuManager instance;
    public GameObject buttons;

    private void Awake()
    {
       instance = this;
        StartCoroutine(ActivateButtons());
    }

    public IEnumerator ActivateButtons()
    {
        yield return new WaitForSeconds(1.5f);
        buttons.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void OpenOptionsMenu(GameObject desiredMenu)
    {
        
        desiredMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void ExitOptionsMenu(GameObject desiredMenu)
    {
        desiredMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    public void OpenHighScoresMenu(GameObject desiredMenu)
    {

        desiredMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void ExitHighScoresMenu(GameObject desiredMenu)
    {
        desiredMenu.SetActive(true);
        highScoreMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
