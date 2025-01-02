using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGaneration : MonoBehaviour
{
    [SerializeField] GameObject Cube;
    bool TF = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (TF == false)
        {
            Instantiate(Cube, this.transform.position, Quaternion.identity);
            TF = true;
        }
    }

}
