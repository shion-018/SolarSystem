using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generation : MonoBehaviour
{

    //生成するゲームオブジェクト
    public GameObject target;
    public Transform position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Instantiate( 生成するオブジェクト,  場所, 回転 );  回転はそのままなら↓
            Instantiate(target, this.transform.position, Quaternion.identity);
        }
    }
}
