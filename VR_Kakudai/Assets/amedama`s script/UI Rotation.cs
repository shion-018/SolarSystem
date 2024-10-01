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
            MainCamera = GetComponent<GameObject>();
        }    
    }


    void LateUpdate()
    {
        //�@�J�����Ɠ��������ɐݒ�
        transform.rotation = MainCamera.transform.rotation;
    }
}
