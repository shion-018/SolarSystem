using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // parent の生成
        var parent = new GameObject("Parent").transform;
        // child の生成
        var child = new GameObject("Child").transform;
        // parent を child の親に設定
        child.SetParent(parent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
