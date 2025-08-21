using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform _rec;

    Vector2 _originSized;

    private void Awake()
    {
        _rec = GetComponent<RectTransform>();
        _originSized = _rec.sizeDelta;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Çï·Î?");
        _rec.sizeDelta = new Vector2(_originSized.x * 1.2f, _originSized.y);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ºüÀÕ?");
        _rec.sizeDelta = _originSized;
    }

    public void ClinkReduction()
    {
        _rec.sizeDelta = _rec.localScale * 0.8f;
    }
}
