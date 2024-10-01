using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class generation : MonoBehaviour
{

    [SerializeField] private GameObject[] target;

    int i = 0;

    int  Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (/*Input.GetKeyDown(KeyCode.Space)*/Count == 0)
        {

            i = Random.Range(0, target.Length);

            //Instantiate( 生成するオブジェクト,  場所, 回転 ); 
            Instantiate(target[i], this.transform.position, Quaternion.identity);
            Count++;
        }
    }
}
