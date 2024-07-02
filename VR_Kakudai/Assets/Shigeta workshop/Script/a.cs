using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // parent ‚Ì¶¬
        var parent = new GameObject("Parent").transform;
        // child ‚Ì¶¬
        var child = new GameObject("Child").transform;
        // parent ‚ğ child ‚Ìe‚Éİ’è
        child.SetParent(parent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
