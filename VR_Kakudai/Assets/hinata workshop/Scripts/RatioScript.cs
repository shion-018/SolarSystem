using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatioScript : MonoBehaviour
{
    public GameObject scoreText;
   Renderer renderer;
    Bounds bounds;
    Vector3 before;
    Vector3 now;
    float ScaleSize;


    void Start()
    {
        renderer = GetComponent<Renderer>();
        bounds = renderer.bounds;
        before = bounds.size;
    }

    void Update()
    {
        bounds = renderer.bounds;
        now = bounds.size;
        ScaleSize = now.x / before.x;
        Text ScaleText = scoreText.GetComponent<Text>();
        ScaleText.text = "Å~" + ScaleSize.ToString();
    }

}
