using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI timerText; 
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
        if (elapsedTime < 30f) return 3;
        if (elapsedTime < 60f) return 2;
        return 1;
    }

    public void SaveResult(bool isWin)
    {
        int stars = isWin ? CalculateStars() : 0;
        string jsonData = JsonUtility.ToJson(new GameResult(elapsedTime, stars, isWin));

        string path = Path.Combine(Application.persistentDataPath, "game_result.json");
        File.WriteAllText(path, jsonData);
        Debug.Log("File saved at: " + Path.Combine(Application.persistentDataPath, "game_result.json"));
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
