using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startGameHItbox : MonoBehaviour
{
    [SerializeField] GameObject GanerateCustomer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GanerateCustomer.gameObject.tag == "Start")
        {
            GanerateCustomer.GetComponent<generation>().enabled = true;
        }
    }
}
