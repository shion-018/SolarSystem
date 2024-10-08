using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emptyseat : MonoBehaviour
{
    public bool[] seat;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i< seat.Length;i++)
        {
            seat[i] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
