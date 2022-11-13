using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{

    public static SaveLoad Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


    }
    public string name1 = "Rimbiy";
    public string name2;
    public string name3;
    public string name4;
    public string name5;
    public int score1 = 1362700;
    public int score2;
    public int score3;
    public int score4;
    public int score5;


    [System.Serializable]
    class SaveData
    {
        public string name1;
        public string name2;
        public string name3;
        public string name4;
        public string name5;
        public int score1;
        public int score2;
        public int score3;
        public int score4;
        public int score5;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        if (GameManager.instance.attemptScore > score1)
        {
            data.score1 = GameManager.instance.attemptScore;
            data.name1 = GameManager.instance.attemptName;
            data.score2 = Instance.score1;
            data.name2 = Instance.name1;
            data.score3 = Instance.score2;
            data.name3 = Instance.name2;
            data.score4 = Instance.score3;
            data.name4 = Instance.name3;
            data.score5 = Instance.score4;
            data.name5 = Instance.name4;
        }
        else if (GameManager.instance.attemptScore < score1 && GameManager.instance.attemptScore > score2)
        {
            data.score1 = Instance.score1;
            data.name1 = Instance.name1;
            data.score2 = GameManager.instance.attemptScore;
            data.name2 = GameManager.instance.attemptName;
            data.score3 = Instance.score2;
            data.name3 = Instance.name2;
            data.score4 = Instance.score3;
            data.name4 = Instance.name3;
            data.score5 = Instance.score4;
            data.name5 = Instance.name4;
        }
        else if (GameManager.instance.attemptScore < score2 && GameManager.instance.attemptScore > score3)
        {
            data.score1 = Instance.score1;
            data.name1 = Instance.name1;
            data.score2 = Instance.score2;
            data.name2 = Instance.name2;
            data.score3 = GameManager.instance.attemptScore;
            data.name3 = GameManager.instance.attemptName;
            data.score4 = Instance.score3;
            data.name4 = Instance.name3;
            data.score5 = Instance.score4;
            data.name5 = Instance.name4;
        }
        else if (GameManager.instance.attemptScore < score3 && GameManager.instance.attemptScore > score4)
        {
            data.score1 = Instance.score1;
            data.name1 = Instance.name1;
            data.score2 = Instance.score2;
            data.name2 = Instance.name2;
            data.score3 = Instance.score3;
            data.name3 = Instance.name3;

            data.score4 = GameManager.instance.attemptScore;
            data.name4 = GameManager.instance.attemptName;
            data.score5 = Instance.score4;
            data.name5 = Instance.name4;
        }
        else if (GameManager.instance.attemptScore < score4 && GameManager.instance.attemptScore > score5)
        {
            data.score1 = Instance.score1;
            data.name1 = Instance.name1;
            data.score2 = Instance.score2;
            data.name2 = Instance.name2;
            data.score3 = Instance.score3;
            data.name3 = Instance.name3;
            data.score4 = Instance.score4;
            data.name4 = Instance.name4;
            data.score5 = GameManager.instance.attemptScore;
            data.name5 = GameManager.instance.attemptName;
        }

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    [ContextMenu("Load")]
    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Instance.score1 = data.score1;
            Instance.score2 = data.score2;
            Instance.score3 = data.score3;
            Instance.score4 = data.score4;
            Instance.score5 = data.score5;
            Instance.name1 = data.name1;
            Instance.name2 = data.name2;
            Instance.name3 = data.name3;
            Instance.name4 = data.name4;
            Instance.name5 = data.name5;
            
        }
    }
}
