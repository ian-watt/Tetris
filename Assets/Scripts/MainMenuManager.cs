
using System.Collections;
using TMPro;
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
    public Canvas fadeObject;

    public TextMeshProUGUI score1;
    public TextMeshProUGUI score2;
    public TextMeshProUGUI score3;
    public TextMeshProUGUI score4;
    public TextMeshProUGUI score5;
    public int devScore = 123600;

    private void Awake()
    {
        instance = this;
        StartCoroutine(ActivateButtons());

        SaveLoad.Instance.LoadScore();
        AssignHighScores();
    }

    private void AssignHighScores()
    {
        if(SaveLoad.Instance.score1 != 0)
        {
            score1.text = "1. " + SaveLoad.Instance.name1 + " - " + SaveLoad.Instance.score1;
        }
        else
        {
            score1.text = "1. EMPTY";
        }
        if (SaveLoad.Instance.score2 != 0)
        {
            score2.text = "2. " + SaveLoad.Instance.name2 + " - " + SaveLoad.Instance.score2;
        }
        else
        {
            score2.text = "2. EMPTY";
        }
        if (SaveLoad.Instance.score3 != 0)
        {
            score3.text = "3. " + SaveLoad.Instance.name3 + " - " + SaveLoad.Instance.score3;
        }
        else
        {
            score3.text = "3. EMPTY";
        }
        if (SaveLoad.Instance.score4 != 0)
        {
            score4.text = "4. " + SaveLoad.Instance.name4 + " - " + SaveLoad.Instance.score4;
        }
        else
        {
            score4.text = "4. EMPTY";
        }
        if (SaveLoad.Instance.score5 != 0)
        {
            score5.text = "5. " + SaveLoad.Instance.name5 + " - " + SaveLoad.Instance.score5;
        }
        else
        {
            score5.text = "5. EMPTY";
        }
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
        fadeObject.sortingOrder = 1;
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
