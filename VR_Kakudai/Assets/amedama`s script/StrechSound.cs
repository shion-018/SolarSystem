

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrechSound : MonoBehaviour
{

    
    [SerializeField] AudioClip sound1;
    [SerializeField] AudioSource audioSource;
    float Dishes_Original_Size_x;
    [SerializeField, Tooltip("‰¹‚ª–Â‚éŠÔŠu")] float magnification = 0.3f;
    float magnification_judge;
    float reduction_judge;

    // Start is called before the first frame update
    void Start()
    {
        Dishes_Original_Size_x = this.transform.localScale.x;
        magnification_judge = 1 + magnification;
        reduction_judge = 1 - magnification;

    }

    // Update is called once per frame
    void Update()
    {
        if(Dishes_Original_Size_x * magnification_judge <= this.transform.localScale.x )
        {
            audioSource.PlayOneShot(sound1);
            Debug.Log(magnification_judge);
            magnification_judge += magnification;
            reduction_judge += magnification ;
            audioSource.pitch += 0.01f; 
        }
        else if (Dishes_Original_Size_x * reduction_judge >= this.transform.localScale.x)
        {
            audioSource.PlayOneShot(sound1);
            Debug.Log(reduction_judge);
            magnification_judge -= magnification;
            reduction_judge -= magnification;
            audioSource.pitch -= 0.01f;
        }
    }

}