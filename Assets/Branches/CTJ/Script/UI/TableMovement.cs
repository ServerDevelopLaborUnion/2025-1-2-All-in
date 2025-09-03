using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class TableMovement : MonoBehaviour
{
    
    [SerializeField] private RectTransform table;

    [SerializeField] float activeXValue = 300.0f;
    [SerializeField] float unActiveXValue = 300.0f;
    [SerializeField] float duration = 1.0f;
    bool isActive = false;

    private void Update()
    {
        if(Keyboard.current.tabKey.wasPressedThisFrame)
        {
            TableSets();
        }
    }

    public void TableSets()
    {
        if (isActive == false)
            ActiveTable();
        else if (isActive == true)
            UnactiveTable();
    }

    public void ActiveTable()
    {
        isActive = true;
        Sequence tableSeq = DOTween.Sequence();

        tableSeq.Append(table.DOAnchorPosX(activeXValue, duration));
    }

    public void UnactiveTable()
    {
        isActive = false;
        Sequence tableSeq = DOTween.Sequence();

        tableSeq.Append(table.DOAnchorPosX(unActiveXValue, duration));
    }
}
