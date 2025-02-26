using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;
    public GameObject manaPrefab;
    public GameObject goldPrefab;

    private void Awake()
    {
        if ( Instance == null )
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void DropLoot(Vector3 position)
    {
        Instantiate(manaPrefab , position + new Vector3(0.2f , 0 , 0) , Quaternion.identity);
        Instantiate(goldPrefab , position + new Vector3(-0.2f , 0 , 0) , Quaternion.identity);
    }
}
