using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyLogUI : MonoBehaviour
{
    public Transform content;        
    public GameObject logTextPrefab;  
    public ScrollRect scrollRect;     
    public int maxLogs = 5;              

    public void AddLog(string message, Color color)
    {
        if (content.childCount >= maxLogs)
        {
            Destroy(content.GetChild(0).gameObject);
        }

        GameObject newLog = Instantiate(logTextPrefab, content);
        TMP_Text txt = newLog.GetComponent<TMP_Text>();
        txt.text = message;
        txt.color = color;

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
