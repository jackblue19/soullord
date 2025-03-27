using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] public GameObject man1Select;
    [SerializeField] public GameObject man1MenuBar;
    [SerializeField] public GameObject man1Play;
    [SerializeField] public GameObject nonStar1, nonStar2, nonStar3;
    [SerializeField] private TextMeshProUGUI goldText;
    private GameTimer gameTimer;
    public GameResult LoadResult()
    {
        string path = Path.Combine(Application.persistentDataPath, "game_result.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            return JsonUtility.FromJson<GameResult>(jsonData);
        }

        return new GameResult(0, 0, false,0);
    }
    private void UpdateTimerUI()
    {
        GameResult gameResult = LoadResult();
        goldText.text = gameResult.gold + " Gold";
    }

    void Start()
    {
        man1Select.SetActive(true);
        man1MenuBar.SetActive(false);
        man1Play.SetActive(false);
        GameResult result = LoadResult();
        UpdateTimerUI();
        UpdateStars(result.stars);
        gameTimer = FindAnyObjectByType<GameTimer>();
    }
    void UpdateStars(int stars)
    {

        nonStar1.SetActive(true);
        nonStar2.SetActive(true);
        nonStar3.SetActive(true);

        if (stars == 3)
        {
            nonStar1.SetActive(false);
            nonStar2.SetActive(false);
            nonStar3.SetActive(false);
        }
        if (stars == 2)
        {
            nonStar1.SetActive(false);
            nonStar2.SetActive(false);
        }
        if (stars == 1)
        {
            nonStar1.SetActive(false);
        }
    }


    public void ShowMan1MenuBar()
    {
        man1MenuBar.SetActive(true);
    }

    public void ShowMan1Play()
    {
        man1Play.SetActive(true);
    }

    public void HiddenMan1MenuBar()
    {
        man1MenuBar.SetActive(false);
    }

    public void HiddenMan1Play()
    {
        man1MenuBar.SetActive(false);
        man1Play.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScence");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
