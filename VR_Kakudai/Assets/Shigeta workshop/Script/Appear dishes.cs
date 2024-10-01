using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appeardishes : MonoBehaviour
{
    [SerializeField] private GameObject dishes;

    //[SerializeField] private Staff staff;

    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnedDishes = Instantiate(dishes, this.transform.position, Quaternion.identity);

        //staff.SetChild(spawnedDishes);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
