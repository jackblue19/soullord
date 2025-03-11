using UnityEngine;

public class Scenemanagerment : Singleton<Scenemanagerment>
{
    public string SceneTransitionName {  get; private set; }

    public void SetTransitionname(string transitionname)
    {
        this.SceneTransitionName = transitionname;
    }
}
