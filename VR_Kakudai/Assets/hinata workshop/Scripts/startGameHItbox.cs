using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startGameHItbox : MonoBehaviour
{
    [SerializeField] GameObject GanerateCustomer;
    public MonoBehaviour testGaneration;
    // Start is called before the first frame update
    void Start()
    {
        testGaneration.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("ganeration‹N“®");
            testGaneration.enabled = true;
        }
    }
}
