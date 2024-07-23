using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultStretch : MonoBehaviour
{

    private Vector3 BeforeScale;
    private Vector3 AfterScale;
    [SerializeField] private float stretch = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        BeforeScale = transform.localScale;
        AfterScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        AfterScale = transform.localScale;

        if (AfterScale != BeforeScale)
        {
            transform.localScale *= stretch;
            
        }

        BeforeScale = transform.localScale;
    }
}
