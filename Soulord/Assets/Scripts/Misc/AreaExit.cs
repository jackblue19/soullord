using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    private bool isUnlocked = false;

    private void Start()
    {
        isUnlocked = false; // Ban đầu cửa bị khóa
    }

    public void UnlockExit()
    {
        isUnlocked = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControllerZ>() != null && isUnlocked)
        {
            SceneManager.LoadScene(sceneToLoad);
            Scenemanagerment.Instance.SetTransitionname(sceneTransitionName);
        }
    }
}
