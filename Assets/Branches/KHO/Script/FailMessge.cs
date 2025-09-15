using System.Collections;
using UnityEngine;

public class FailMessge : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        StartCoroutine(Onoff());
    }

    private IEnumerator Onoff()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
