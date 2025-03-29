using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DishesFusion : MonoBehaviour
{

    [SerializeField] int[] Number = new int[3];

     int DishesNumber = 999;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        switch ((int)this.gameObject.transform.localScale.x)
        {
            case 0:
                DishesNumber = Number[0];
                break;

            case 1:
                DishesNumber = Number[1];
                break;

            case 2:
                DishesNumber = Number[2];
                break;
        }
    }
}
