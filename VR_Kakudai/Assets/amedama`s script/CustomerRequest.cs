using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerRequest : MonoBehaviour
{

    //Inspectorで設定してほしいところ
    [SerializeField] private GameObject[] RequestDishes;//料理の種類
    [SerializeField] private Sprite[] DishesSprits;//料理の画像
    [SerializeField] private Sprite[] SizeSpecificationSprits;//大きさ指定の画像
    [SerializeField] private Image DishesImage;//料理の画像を映す場所の設定
    [SerializeField] private Image SizeImage;//大きさの画像を映す場所の設定

    private int DishesSpriteValue; private int DishesSpriteValueMin; private int DishesSpriteValueMax;
    private int SizeSpecificationSpriteValue; private int SizeSpecificationSpriteValueMin; private int SizeSpecificationSpriteValueMax;


    // Start is called before the first frame update
    void Start()
    {
        //乱数生成の上限と下限を設定
        DishesSpriteValueMin = 0;
        DishesSpriteValueMax = RequestDishes.Length;
        SizeSpecificationSpriteValueMin = 0;
        SizeSpecificationSpriteValueMax = SizeSpecificationSprits.Length;

        //乱数生成で料理と大きさを決定
        DishesSpriteValue = Random.Range(DishesSpriteValueMin, DishesSpriteValueMax);
        DishesImage.sprite = DishesSprits[DishesSpriteValue];
        SizeSpecificationSpriteValue = Random.Range(SizeSpecificationSpriteValueMin, SizeSpecificationSpriteValueMax);
        SizeImage.sprite = SizeSpecificationSprits[SizeSpecificationSpriteValue];

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider colliderDishes)
    {

        //当たったオブジェクトを判定
        if (colliderDishes.tag == RequestDishes[DishesSpriteValue].tag)//料理の種類をtagで判定
        {

            if (SizeSpecificationSpriteValue < colliderDishes.transform.localScale.x)//料理の大きさをScale.xで判定
            {
                Destroy(colliderDishes.gameObject);
            }
        }
    }
}
