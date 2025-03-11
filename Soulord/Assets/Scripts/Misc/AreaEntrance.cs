using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    void Start()
    {
        if (transitionName == Scenemanagerment.Instance.SceneTransitionName)
        {
            PlayerControllerZ.Instance.transform.position = this.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
