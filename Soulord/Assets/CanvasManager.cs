using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] public GameObject man1Select;
    [SerializeField] public GameObject man1MenuBar;
    [SerializeField] public GameObject man1Play;

    void Start()
    {
        man1Select.SetActive(true);
        man1MenuBar.SetActive(false);
        man1Play.SetActive(false);
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
