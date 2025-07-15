using System.Collections;
using UnityEngine;

public class SelectChecker : MonoBehaviour
{
    [SerializeField] GameObject Test;

    SpriteRenderer _sr;

    public bool IsActive { get; private set; } = false;
    bool canMining = true;

    [Header("Durability")]
    [SerializeField] int lv1Dura = 8;
    [SerializeField] int lv2Dura = 5;
    [SerializeField] int lv3Dura = 2;

    

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        Test.SetActive(false);
    }

    public void GetActive()
    {
        IsActive = true;
        Test.SetActive(true);
    }

    public void ExitActive()
    {
        IsActive = false;
        Test.SetActive(false);
    }

    public void Mining()
    {
        if (canMining)
        {
            DurabilityChecker();
            StartCoroutine(MiningDeley());
        }
    }

    private void DurabilityChecker()
    {
        lv1Dura--;

        if (lv1Dura == 0 || lv2Dura == 0 || lv3Dura ==0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator MiningDeley()
    {
        _sr.color = Color.red;
        canMining = false;
        yield return new WaitForSeconds(0.3f);
        _sr.color = Color.white;
        canMining = true;
    }
    
}
