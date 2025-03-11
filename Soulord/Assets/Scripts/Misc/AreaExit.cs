using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControllerZ>() != null)
        {
            SceneManager.LoadScene(sceneToLoad);
            Scenemanagerment.Instance.SetTransitionname(sceneTransitionName);
        }
    }
}
