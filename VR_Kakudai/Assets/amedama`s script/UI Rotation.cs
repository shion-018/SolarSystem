using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour
{

    [SerializeField] private GameObject MainCamera;


    void LateUpdate()
    {
        //@ƒJƒƒ‰‚Æ“¯‚¶Œü‚«‚Éİ’è
        transform.rotation = MainCamera.transform.rotation;
    }
}
