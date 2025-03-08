using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float restoreDefaultMatime = .1f;

    private Material defaultMat;
    private SpriteRenderer whiteRenderer;
    private EnemyAIL EnemyAIL;
    private void Awake()
    {
        EnemyAIL = GetComponent<EnemyAIL>();
        whiteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = whiteRenderer.material;
    }

    public IEnumerator FlashRountine()
    {
        whiteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(restoreDefaultMatime);
        whiteRenderer.material = defaultMat;
        EnemyAIL.Die();
    }
}
