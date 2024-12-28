using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    public Transform playerCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerCamera != null)
        {
            // 常にカメラの方向を向くようにする
            transform.LookAt(playerCamera);

            // 必要に応じてオブジェクトを反転（UIが裏返るのを防ぐ）
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        }
    }
}
