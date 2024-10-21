using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alphakey : MonoBehaviour
{

    [SerializeField] GameObject ebiebi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(ebiebi,this.transform.position + Vector3.forward,Quaternion.identity);
            Debug.Log("Space");
        }
    }
}
