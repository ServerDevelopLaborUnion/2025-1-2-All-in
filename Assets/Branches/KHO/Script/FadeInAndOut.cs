using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class FadeInAndOut : MonoBehaviour
{
    public Image image;
    public float _speed = 3f;

    private void Start()
    {
        Color color  = new Color(0, 0, 0, 0f);
        image.color = color;
    }

    public IEnumerator StartFadeIn()
    {
        yield return StartCoroutine(FadeIn());
    }

    public IEnumerator StartFadeStart()
    {
        yield return StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        float alpha = 1f;
        Color color  = new Color(0, 0, 0, 1f);
        image.color = color;
        
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime  * _speed;
            color.a = Mathf.Clamp01(alpha);
            image.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float alpha = 0f;
        Color color = new Color(0, 0, 0, 0);
        image.color = color;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * _speed;
            color.a = Mathf.Clamp01(alpha);
            image.color = color;
            yield return null;
        }
    }

}
