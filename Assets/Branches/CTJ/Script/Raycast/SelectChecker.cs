using System.Collections;
using UnityEngine;

public class SelectChecker : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] GameObject Test;

    public bool IsActive { get; private set; } = false;

    bool canMining = true;
    [SerializeField] float miningDeley = 0.5f;

    [Header("SOs")]
    public int OreSONumber = 0;
    [SerializeField] OreSO[] OreSOArray;



    bool noDurability;
    int dura;
    GameObject collectiblesItem;
    int maxDrop;


    private void Awake()
    {
        SetingObject();
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
        if (!noDurability)
        {
            dura--;

            if (dura == 0)
            {
                Droper();
            }
        }
        else
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

    private void SetingObject()
    {
        OreSO thisOreSO = OreSOArray[OreSONumber];

        gameObject.name = thisOreSO.oreName;
        _sr.sprite = thisOreSO.OreSprite;

        noDurability = thisOreSO.noDurability;
        dura = thisOreSO.dura;
        collectiblesItem = thisOreSO.collectiblesItem;
        maxDrop = thisOreSO.maxDrop;
    }

    private void OnValidate()
    {
        SetingObject();
    }
}
