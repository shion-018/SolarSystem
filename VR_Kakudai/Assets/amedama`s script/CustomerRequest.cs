using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;


public class CustomerRequest : MonoBehaviour
{

    //Inspectorで設定してほしいところ
    [SerializeField] private GameObject[] RequestDishes;//料理の種類
    [SerializeField] private Sprite[] SizeSpecificationSprits;//大きさ指定の画像
    [SerializeField] private Sprite[] HighSpecificationSprits;//高さ指定の画像
    [SerializeField] private UnityEngine.UI.Image DishesImage;//料理の画像を映す場所の設定
    [SerializeField] private UnityEngine.UI.Image SizeImage;//大きさの画像を映す場所の設定・高さも

    [SerializeField] private GameObject SalesAmountText;//売上金額の合計を映すところ

    [SerializeField] AudioClip SuccesSound;
    [SerializeField] AudioClip MissSound;
    [SerializeField] AudioSource audioSource;

    [SerializeField] private GameObject UI_matome;
    [SerializeField] private Transform contect;


    private int DishesValue; private int DishesValueMin; private int DishesValueMax;
    private int SizeSpecificationSpriteValue; private int SizeSpecificationSpriteValueMin; private int SizeSpecificationSpriteValueMax;
    private int RequestDishesPrice = 0;
    private int[] DishesMagPrice;
    private int SalesAmount = 0;
    [SerializeField] private int AssortDishesNumberMax = 3;//最大の料理組み合わせ数 + 1
    private int[] AssortDishesNumber;
    private DishesSetting dishesSetting;
    private AmountText amountText;
    private DishesFusion dishesFusion;
    private int Assort;
    private int AssortJudge = 0;


    [SerializeField]private int aaa = 1;//仮
    private int HighJudgeNumber;

    GameObject A;
    GameManager gameManager;
    

    // Start is called before the first frame update
    void Start()
    {



        Sprite[] DishesSprits = new Sprite[RequestDishes.Length];//料理の画像        
        int[] DishesPrice = new int[RequestDishes.Length];
        Assort = Random.Range(1, AssortDishesNumberMax);
        AssortDishesNumber = new int[Assort];

        Transform[] UI_matome_Hairetu = new Transform[Assort];
        Image[,] UI_Images = new Image[Assort, 3];//料理の画像を表示するUI

        A = GameObject.Find("GameManager");
        gameManager = A.GetComponent<GameManager>();

        for (int i = 0; i < UI_matome_Hairetu.Length; i++)
        {
            UI_matome_Hairetu[i] = Instantiate(UI_matome, contect).transform;
            
            for(int j = 0;j < UI_Images.GetLength(1);j++)
            {
 
                UI_Images[i,j] = UI_matome_Hairetu[i].transform.GetChild(j).GetComponent<Image>();
               
            }
        }



        for (int DishesNumber = 0; DishesNumber < RequestDishes.Length; DishesNumber++)
        {

            dishesSetting = RequestDishes[DishesNumber].GetComponent<DishesSetting>();
            SalesAmountText = GameObject.FindWithTag("SalesAmountText");
            amountText = SalesAmountText.GetComponent<AmountText>();

            DishesSprits[DishesNumber] = dishesSetting.DishesSprite;
            DishesPrice[DishesNumber] = dishesSetting.DishesPrice;



        }


        //Swich文で処理変更
        switch (aaa)
        {
            case 0://全ての中からランダムな数盛り付け


                //乱数生成の上限と下限を設定
                DishesValueMin = 0;
                DishesValueMax = RequestDishes.Length;
                SizeSpecificationSpriteValueMin = 0;
                SizeSpecificationSpriteValueMax = SizeSpecificationSprits.Length;
                int[] DishesNotSame = new int[3] { -1, -1, -1 };
                bool NotSameJudge = true;


                //乱数生成で料理と大きさを決定
                for (int i = 0; i < Assort; i++)
                {
                    do
                    {

                        NotSameJudge = true;
                        DishesValue = Random.Range(DishesValueMin, DishesValueMax);

                        for (int j = 0; j < Assort; j++)
                        {
                            if (DishesNotSame[j] != DishesValue && DishesNotSame[j] == -1)
                            {
                                DishesNotSame[i] = DishesValue;
                                break;
                            }
                            else if (DishesNotSame[j] == DishesValue)
                            {
                                NotSameJudge = false;
                            }
                        }
                    } while (!NotSameJudge);

                    dishesSetting = RequestDishes[DishesValue].GetComponent<DishesSetting>();
                    UI_Images[i, 0].sprite = DishesSprits[DishesValue];
                    RequestDishesPrice = DishesPrice[DishesValue];
                    SizeSpecificationSpriteValue = Random.Range(SizeSpecificationSpriteValueMin, SizeSpecificationSpriteValueMax);
                    UI_Images[i, 1].sprite = SizeSpecificationSprits[SizeSpecificationSpriteValue];

                    AssortDishesNumber[i] = dishesSetting.Number[SizeSpecificationSpriteValue];

                    Debug.Log("盛り付け : " + AssortDishesNumber[i]);
                }

                break;


            case 1://高さを指定      

                //乱数生成の上限と下限を設定
                DishesValueMin = 0;
                DishesValueMax = RequestDishes.Length;
                SizeSpecificationSpriteValueMin = 0;
                SizeSpecificationSpriteValueMax = HighSpecificationSprits.Length;

                //乱数生成で料理と大きさを決定
                DishesValue = Random.Range(DishesValueMin, DishesValueMax);
                dishesSetting = RequestDishes[DishesValue].GetComponent<DishesSetting>();
                UI_Images[0,0].sprite = DishesSprits[DishesValue];
                RequestDishesPrice = DishesPrice[DishesValue];
                SizeSpecificationSpriteValue = Random.Range(SizeSpecificationSpriteValueMin, SizeSpecificationSpriteValueMax);
                UI_Images[0,1].sprite = HighSpecificationSprits[SizeSpecificationSpriteValue];


                AssortDishesNumber[0] = dishesSetting.Number[SizeSpecificationSpriteValue];

                HighJudgeNumber = (int)Random.Range(1.0f, 2.0f);
                Debug.Log("盛り付け : " + AssortDishesNumber[0]);

                break;

            case 2://一つだけBIG!!


                //乱数生成の上限と下限を設定
                DishesValueMin = 0;
                DishesValueMax = RequestDishes.Length;
                SizeSpecificationSpriteValueMin = 0;
                SizeSpecificationSpriteValueMax = SizeSpecificationSprits.Length;

                //乱数生成で料理と大きさを決定
                

                    DishesValue = Random.Range(DishesValueMin, DishesValueMax);
                    dishesSetting = RequestDishes[DishesValue].GetComponent<DishesSetting>();
                    DishesImage.sprite = DishesSprits[DishesValue];
                    RequestDishesPrice = DishesPrice[DishesValue];
                    //SizeSpecificationSpriteValue = Random.Range(SizeSpecificationSpriteValueMin, SizeSpecificationSpriteValueMax);
                    SizeImage.sprite = SizeSpecificationSprits[2];

                    AssortDishesNumber[0] = dishesSetting.Number[2];

                    Debug.Log("盛り付け : " + AssortDishesNumber[0]);
                

                for (int i = 1; i < Assort; i++)
                {

                    DishesValue = Random.Range(DishesValueMin, DishesValueMax);
                    dishesSetting = RequestDishes[DishesValue].GetComponent<DishesSetting>();
                    DishesImage.sprite = DishesSprits[DishesValue];
                    RequestDishesPrice = DishesPrice[DishesValue];
                    //SizeSpecificationSpriteValue = Random.Range(SizeSpecificationSpriteValueMin, SizeSpecificationSpriteValueMax);
                    SizeImage.sprite = SizeSpecificationSprits[0];

                    AssortDishesNumber[i] = dishesSetting.Number[0];

                    Debug.Log("盛り付け : " + AssortDishesNumber[i]);
                }

                break;

        }

    }

    void OnCollisionEnter(Collision colliderDishes)
    {
        if (colliderDishes.gameObject.tag == "donburi")
        {

            //DishesSetting ColDishesSetting = colliderDishes.gameObject.GetComponent<DishesSetting>();
            HighJudge highJudge = colliderDishes.gameObject.GetComponentInChildren<HighJudge>();
            AttachSushiChildrenInRange ASCIR = colliderDishes.gameObject.GetComponent<AttachSushiChildrenInRange>();

            for (int i = 0; i < Assort; i++)
            {
                for (int j = 0; j < Assort; j++)
                {
                    if (AssortDishesNumber[i] == ASCIR.DonburiDishes[j])//料理が正しいかを判定
                    {

                        AssortDishesNumber[i] = -1;
                        AssortJudge += 1;

                    }
                }
            }

            switch (aaa)
            {
                case 0:

                    if (AssortJudge == Assort)
                    {
                        amountText.Amount(DishesMagnification(colliderDishes));

                        gameManager.betogether = true;
                        audioSource.PlayOneShot(SuccesSound);
                        gameManager.SatisfiedCustomers++;
                        Debug.Log( "gM.SC = " + gameManager.SatisfiedCustomers);
                        Destroy(colliderDishes.gameObject);
                        
                    }
                    else
                    {
                        audioSource.PlayOneShot(MissSound);
                    }
                    break;

                case 1:


                    if (highJudge.satisfyHeight >= HighJudgeNumber)
                    {

                        amountText.Amount(DishesMagnification(colliderDishes));
                        gameManager.betogether = true;
                        gameManager.SatisfiedCustomers++;
                        Destroy(colliderDishes.gameObject);

                    }
                    break;

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
