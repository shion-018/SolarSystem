using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class generation : MonoBehaviour
{

    [SerializeField] private GameObject[] target;

    int i = 0;

    GameObject A;
    CustomerCounter CustmerCounter;

    // Start is called before the first frame update
    void Start()
    {
        A = GameObject.Find("CustonerCount");
        CustmerCounter = A.GetComponent<CustomerCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CustmerCounter.counter != 3)
        {
            i = Random.Range(0, target.Length);

            //Instantiate( ��������I�u�W�F�N�g,  �ꏊ, ��] ); 
            Instantiate(target[i], this.transform.position, Quaternion.identity);
            
            CustmerCounter.counter++;
        }



    }
}
