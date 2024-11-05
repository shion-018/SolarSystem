using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CustomerRequest : MonoBehaviour
{

    //Inspectorで設定してほしいところ
    [SerializeField] private GameObject[] RequestDishes;//料理の種類
    [SerializeField] private Sprite[] SizeSpecificationSprits;//大きさ指定の画像
    [SerializeField] private UnityEngine.UI.Image DishesImage;//料理の画像を映す場所の設定
    [SerializeField] private UnityEngine.UI.Image SizeImage;//大きさの画像を映す場所の設定
    [SerializeField] private GameObject SalesAmountText;//売上金額の合計を映すところ

    private int DishesValue; private int DishesValueMin; private int DishesValueMax;
    private int SizeSpecificationSpriteValue; private int SizeSpecificationSpriteValueMin; private int SizeSpecificationSpriteValueMax;
    private int RequestDishesPrice = 0;
    private int[] DishesMagPrice;
    private int SalesAmount = 0;
    private int AssortDishesNumberMax = 4;//最大の料理組み合わせ数 + 1
    private int[] AssortDishesNumber;
    private DishesSetting dishesSetting;
    private AmountText amountText;
    private DishesFusion dishesFusion;
    private int Assort;

    // Start is called before the first frame update
    void Start()
    {



        Sprite[] DishesSprits = new Sprite[RequestDishes.Length];
        int[] DishesPrice = new int[RequestDishes.Length];

        Assort = Random.Range(1, AssortDishesNumberMax);
        AssortDishesNumber = new int[Assort];

        Debug.Log(Assort);


        for (int DishesNumber = 0; DishesNumber < RequestDishes.Length; DishesNumber++)
        {

            dishesSetting = RequestDishes[DishesNumber].GetComponent<DishesSetting>();
            SalesAmountText = GameObject.FindWithTag("SalesAmountText");
            amountText = SalesAmountText.GetComponent<AmountText>();

            DishesSprits[DishesNumber] = dishesSetting.DishesSprite;
            DishesPrice[DishesNumber] = dishesSetting.DishesPrice;



        }

        //乱数生成の上限と下限を設定
        DishesValueMin = 0;
        DishesValueMax = RequestDishes.Length;
        SizeSpecificationSpriteValueMin = 0;
        SizeSpecificationSpriteValueMax = SizeSpecificationSprits.Length;

        //乱数生成で料理と大きさを決定
        for (int i = 0; i < Assort; i++)
        {

            DishesValue = Random.Range(DishesValueMin, DishesValueMax);
            dishesSetting = RequestDishes[DishesValue].GetComponent<DishesSetting>();
            DishesImage.sprite = DishesSprits[DishesValue];
            RequestDishesPrice = DishesPrice[DishesValue];
            SizeSpecificationSpriteValue = Random.Range(SizeSpecificationSpriteValueMin, SizeSpecificationSpriteValueMax);
            SizeImage.sprite = SizeSpecificationSprits[SizeSpecificationSpriteValue];

            AssortDishesNumber[i] = dishesSetting.Number[SizeSpecificationSpriteValue];

            Debug.Log("盛り付け : " + AssortDishesNumber[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision colliderDishes)
    {
        DishesSetting ColDishesSetting = colliderDishes.gameObject.GetComponent<DishesSetting>();


        

        //当たったオブジェクトを判定
        if (ColDishesSetting.DishesNumber == dishesSetting.Number[SizeSpecificationSpriteValue])//料理が正しいかを判定
        {

            if (SizeSpecificationSpriteValue <= colliderDishes.transform.localScale.x)//満足する大きさかの判定
            {

                amountText.Amount(DishesMagnification(colliderDishes));
                
                Destroy(colliderDishes.gameObject);
                
            }
        }
    }

    int DishesMagnification(Collision colliderDishes)
    {

        float RDP = RequestDishesPrice;
        float PM = 0;//PriceMagnification



        for (int i = 0; i < dishesSetting.dishesMagnifications.Length; i++)
        {

            if (colliderDishes.transform.localScale.x >= dishesSetting.dishesMagnifications[i].PriceMagnification[0])
            {
                PM = dishesSetting.dishesMagnifications[i].PriceMagnification[1];
            }

        }

        RDP *= PM;

        return (int)RDP;


    }
}
