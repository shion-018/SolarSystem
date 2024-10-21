using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alphakey : MonoBehaviour
{

    [SerializeField] GameObject ebiebi;
    [SerializeField] GameObject ebiday;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(ebiebi,ebiday.transform.position,Quaternion.identity);
            Debug.Log("Space");
        }
    }
}
