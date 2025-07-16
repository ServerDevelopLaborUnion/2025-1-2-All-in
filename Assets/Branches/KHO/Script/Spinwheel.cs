using System.Collections;
using UnityEngine;

public class Spinwheel : MonoBehaviour
{
    //[SerializeField]private int rotation = 30;
    private Vector3 rotaionV= new UnityEngine.Vector3(0,0,0);

    public void StartRotation()
    {
        StartCoroutine(Roatioing(30));
    }
    public IEnumerator Roatioing(int agle = 20)
    {
        while (rotaionV.z != agle)
        {
            rotaionV.z += 1;
            transform.rotation = Quaternion.Euler(rotaionV);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
