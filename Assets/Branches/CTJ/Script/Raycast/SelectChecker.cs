using System.Collections;
using UnityEngine;

public class SelectChecker : MonoBehaviour
{
    [SerializeField] GameObject Test;

    public bool IsActive { get; private set; } = false;
    bool canMining = true;

    [SerializeField] float miningDeley = 0.5f;

    [Header("Durability")]
    [SerializeField] int lv1Dura = 8;
    [SerializeField] int lv2Dura = 5;
    [SerializeField] int lv3Dura = 2;

    [Header("Collectibles")]
    [SerializeField] GameObject collectiblesItem;
    [SerializeField] int maxDrop = 3;


    private void Awake()
    {
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
            Droper();
        }
    }

    private void Droper()
    {
        int dropItemNumber = Random.Range(1, maxDrop + 1);

        for(int i = 0; i < dropItemNumber; i++)
        {
            GameObject cItem = Instantiate(collectiblesItem);
            Deployer cItemDeployer = cItem.GetComponent<Deployer>();

            cItem.gameObject.transform.position = gameObject.transform.position;
            cItemDeployer.Deploy();
        }
        Destroy(gameObject);
    }

    IEnumerator MiningDeley()
    {
        canMining = false;
        yield return new WaitForSeconds(miningDeley);
        canMining = true;
    }
    
}
