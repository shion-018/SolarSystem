using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndGame()
    {
        if (Application.isEditor)
        {

#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;//ƒQ[ƒ€ƒvƒŒƒCI—¹
#endif        
        }
        else
        {
            Application.Quit();//ƒQ[ƒ€ƒvƒŒƒCI—¹
        }
    }
}
