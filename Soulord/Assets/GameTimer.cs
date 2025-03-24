using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : Singleton<GameTimer>
{
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject nonStar1, nonStar2, nonStar3;
    [SerializeField] private TextMeshProUGUI goldText;
    private float elapsedTime = 0f;
    private bool isRunning = false;
    private bool hasMoved = false;

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        if (!hasMoved)
        {
            hasMoved = true;
            isRunning = true;
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }

    public int CalculateStars()
    {
        if (elapsedTime < 60f) return 3;
        if (elapsedTime < 90f) return 2;
        if (elapsedTime < 120f) return 1;
        return 0;
    }

    public void SaveResult(bool isWin)
    {
        int stars = isWin ? CalculateStars() : 0;
        string jsonData = JsonUtility.ToJson(new GameResult(elapsedTime, stars, isWin));

        string path = Path.Combine(Application.persistentDataPath, "game_result.json");
        File.WriteAllText(path, jsonData);
        Debug.Log("File saved at: " + Path.Combine(Application.persistentDataPath, "game_result.json"));
    }

    public void LoadMenuScene()
    {
        Time.timeScale = 1;
        DestroyDontDestroyOnLoadObjects(); 
        SceneManager.LoadScene("MainMenuBar");
    }

    private void DestroyDontDestroyOnLoadObjects()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == "DontDestroyOnLoad") 
            {
                Destroy(obj);
            }
        }
    }
    public void ShowSuccessScreen(bool isWin)
    {
        successPanel.SetActive(true);
        Time.timeScale = 0;
        int stars = isWin ? CalculateStars() : 0;

        // Reset trạng thái ban đầu
        nonStar1.SetActive(false);
        nonStar2.SetActive(false);
        nonStar3.SetActive(false);

        if (stars == 3)
        {
            goldText.text = "+1000 Gold";
        }
        else if (stars == 2)
        {
            nonStar3.SetActive(true);
            goldText.text = "+1000 Gold";
        }
        else if (stars == 1)
        {
            nonStar2.SetActive(true);
            nonStar3.SetActive(true);
            goldText.text = "+1000 Gold";
        }
        else // Nếu thua
        {
            nonStar1.SetActive(true);
            nonStar2.SetActive(true);
            nonStar3.SetActive(true);
            goldText.text = "+0 Gold";
        }
    }
}


[System.Serializable]
public class GameResult
{
    public float time;
    public int stars;
    public bool isWin;

    public GameResult(float time, int stars, bool isWin)
    {
        this.time = time;
        this.stars = stars;
        this.isWin = isWin;
    }
}
