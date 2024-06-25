using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visit : MonoBehaviour
{

    [SerializeField] private Transform target;
    
    private float speed = 3.0f;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
