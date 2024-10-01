
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrechSound : MonoBehaviour
{

    
    [SerializeField] AudioClip sound1;
    [SerializeField] AudioSource audioSource;
    float Dishes_Original_Size_x;
    float magnification = 1.3f;
    float magnification_judge;

    // Start is called before the first frame update
    void Start()
    {
        Dishes_Original_Size_x = this.transform.localScale.x;
        magnification_judge = magnification;
    }

    // Update is called once per frame
    void Update()
    {
        if(Dishes_Original_Size_x * magnification_judge <= this.transform.localScale.x )
        {
            audioSource.PlayOneShot(sound1);
            Debug.Log(magnification);
            magnification_judge += magnification - 1f;
            audioSource.pitch += 0.01f; 
        }
    }
}