using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestClock : MonoBehaviour
{
    Image clockImage;

    private void Start()
    {
        clockImage = GetComponent<Image>();
    }
    public void UpdateClock(float second)
    {
        clockImage.fillAmount = second;
    }
}
