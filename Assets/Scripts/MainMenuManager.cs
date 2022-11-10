using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject highScoreMenu;
    public static MainMenuManager instance;
    public GameObject buttons;
    public GameObject logo;
    public AudioClip menuButtonSFX;
    public AudioSource audioManager;
    public GameObject musicCheck;
    public GameObject sfxCheck;
    private bool sfxActive = true;
    private bool bgmActive = true;
    public Animator fadeController;

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
        StartCoroutine(FadeOutAndSwitch(index));
        fadeController.SetBool("fade", true);
        
    }

    public IEnumerator FadeOutAndSwitch(int index)
    {
        buttons.SetActive(false);
        yield return new WaitForSeconds(3);
        
        SceneManager.LoadScene(index);
        
    }

    public void OpenOptionsMenu(GameObject desiredMenu)
    {
        
        desiredMenu.SetActive(true);
        mainMenu.SetActive(false);
        logo.SetActive(false);
        if (sfxActive)
        {
            audioManager.PlayOneShot(menuButtonSFX, 1.5f);

        }
    }
    public void ExitOptionsMenu(GameObject desiredMenu)
    {
        desiredMenu.SetActive(true);
        optionsMenu.SetActive(false);
        logo.SetActive(true);
        if (sfxActive)
        {
            audioManager.PlayOneShot(menuButtonSFX, 1.5f);

        }
    }
    public void OpenHighScoresMenu(GameObject desiredMenu)
    {

        desiredMenu.SetActive(true);
        mainMenu.SetActive(false);
        logo.SetActive(false);
        if (sfxActive)
        {
            audioManager.PlayOneShot(menuButtonSFX, 1.5f);

        }
    }
    public void ExitHighScoresMenu(GameObject desiredMenu)
    {
        desiredMenu.SetActive(true);
        highScoreMenu.SetActive(false);
        logo.SetActive(true);
        if (sfxActive)
        {
            audioManager.PlayOneShot(menuButtonSFX, 1.5f);

        }
    }

    public void QuitGame()
    {
        if (sfxActive)
        {
            audioManager.PlayOneShot(menuButtonSFX, 1.5f);

        }
        Application.Quit();
    }
    
    public void ToggleBGM()
    {
        if (bgmActive)
        {
            audioManager.volume = 0;
            bgmActive = false;
            musicCheck.SetActive(false);
        }
        else
        {
            audioManager.volume = .3f;
            bgmActive=true;
            musicCheck.SetActive(true);

        }
    }

    public void ToggleSFX()
    {
        if (sfxActive)
        {
            sfxActive = false;
            sfxCheck.SetActive(false);

        }
        else
        {
            sfxActive = true;
            sfxCheck.SetActive(true);

        }
    }

}
