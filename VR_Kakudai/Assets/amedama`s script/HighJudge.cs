using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJudge : MonoBehaviour
{

    [SerializeField] private int HighJudgeHeight;
    public int satisfyHeight = 0;
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(this.transform.position.x, HighJudgeHeight, this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }



    private void OnTriggerEnter(Collider other)
    {
        satisfyHeight = HighJudgeHeight;

    }
    private void OnTriggerExit(Collider other)
    {
        satisfyHeight = 0;

    }
}
