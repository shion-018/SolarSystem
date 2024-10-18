using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appeardishes : MonoBehaviour
{
    [SerializeField] private GameObject dishes;

    [SerializeField] private GameObject[] point;

    public int dishesnum = 0;

    public bool[] spawndishes = new bool[3]; // 配列のサイズを明示的に3に設定

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawndishes.Length; i++)
        {
            spawndishes[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawndishes.Length >= 3)
        {
            if (spawndishes[0] && point[0] != null)
            {
                Instantiate(dishes, point[0].transform.position, Quaternion.identity);
                spawndishes[0] = false;
            }
            if (spawndishes[1] && point[1] != null)
            {
                Instantiate(dishes, point[1].transform.position, Quaternion.identity);
                spawndishes[1] = false;
            }
            if (spawndishes[2] && point[2] != null)
            {
                Instantiate(dishes, point[2].transform.position, Quaternion.identity);
                spawndishes[2] = false;
            }
        }
    }
}
