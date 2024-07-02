using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerRequest : MonoBehaviour
{

    [SerializeField] private GameObject[] RequestDishes;
    [SerializeField] private Sprite[] ScreenSprits;
    [SerializeField] private Image RequestImage;

    private int SpriteValue;
    private int SpriteValueMin;
    private int SpriteValueMax; 


    // Start is called before the first frame update
    void Start()
    {
        SpriteValueMin = 0;
        SpriteValueMax = RequestDishes.Length;

        SpriteValue = Random.Range(SpriteValueMin, SpriteValueMax);
        RequestImage.sprite = ScreenSprits[SpriteValue];
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider colliderDishes)
    {

        //当たったオブジェクトを判定
        if (colliderDishes.tag == RequestDishes[SpriteValue].tag)
        {
            Destroy(colliderDishes.gameObject);
        }
    }
}
