using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class generation : MonoBehaviour
{

    [SerializeField] private GameObject[] target;

    int i = 0;

    int  Count = 0;

    GameObject A;
    emptyseat empty;

    // Start is called before the first frame update
    void Start()
    {
        A = GameObject.Find("emptyseat");
        empty = A.GetComponent<emptyseat>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < empty.seat.Length; i++)
        {
            if (empty.seat[i])
            {
                i = Random.Range(0, target.Length);

                //Instantiate( ��������I�u�W�F�N�g,  �ꏊ, ��] ); 
                Instantiate(target[i], this.transform.position, Quaternion.identity);
                //Count++;
            }
        }
    }
}
