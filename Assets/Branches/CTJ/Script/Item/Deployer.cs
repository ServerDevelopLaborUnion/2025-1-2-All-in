using System.Collections;
using UnityEngine;

public class Deployer : MonoBehaviour
{
    [SerializeField] private float dropForce = 0.4f;

    public void Deploy()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float randomPower = Random.Range(dropForce * 0.1f, dropForce * 0.2f);
        StartCoroutine(Deployed(gameObject.transform, randomDir * randomPower));
    }

    IEnumerator Deployed(Transform tf, Vector2 totalMove)
    {
        Vector2 moved = Vector2.zero;

        while (moved.magnitude < totalMove.magnitude)
        {
            Vector2 m = totalMove.normalized * 10 * Time.fixedDeltaTime;
            tf.Translate(m);
            moved += m;
            yield return new WaitForFixedUpdate();
        }
    }
}
