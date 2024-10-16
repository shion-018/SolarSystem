using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour
{

    [SerializeField] private GameObject MainCamera;

    void Start()
    {
        if(MainCamera == null)
        {
            MainCamera = Camera.main.gameObject;
        }    
    }


    void LateUpdate()
    {
        //　カメラと同じ向きに設定
        transform.rotation = MainCamera.transform.rotation;
    }
}
