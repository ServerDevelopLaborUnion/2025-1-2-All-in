using System.Collections;
using UnityEngine;

public class Grow : MonoBehaviour
{
    private SpriteRenderer sprite;
    private int growing = 0;
    [SerializeField]public GrowingSO so;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(Growing());
    }

    private IEnumerator Growing()
    {
        while (growing < so.sprites.Length)
        {
            sprite.sprite = so.sprites[growing];
            growing++;

            if (growing >= so.sprites.Length)
                yield break; 

            yield return new WaitForSeconds(so.time);
        }
    }
}

