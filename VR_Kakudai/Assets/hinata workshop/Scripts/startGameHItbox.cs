using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startGameHItbox : MonoBehaviour
{
    [SerializeField] GameObject GanerateCustomer;
    public MonoBehaviour Ganeration;
    // Start is called before the first frame update
    void Start()
    {
        Ganeration.enabled = false;
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
            Ganeration.enabled = true;
        }
    }
}
