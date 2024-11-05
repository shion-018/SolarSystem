using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishesSetting : MonoBehaviour   
{

   // [SerializeField] public GameObject Dishes;
    [SerializeField] public Sprite DishesSprite;
    [SerializeField] public int DishesPrice = 700;
    public int[] Number = new int[3];

    [HideInInspector] public int DishesNumber = 999;
    //[SerializeField] public float[,] DishesMagnificationPrice;//ägëÂÇµÇΩéûÇÃÅAíiäKÇ∆î{ó¶ÇåàÇﬂÇƒÇŸÇµÇ¢Ç≈Ç∑ÅB


    public DishesMagnification[] dishesMagnifications = new DishesMagnification[3];
    [System.Serializable] public class DishesMagnification { public float[] PriceMagnification = new float[2]{1,1}; }


    // Start is called before the first frame update
    void Start()
    {
        

        switch ((int)this.gameObject.transform.localScale.x)
        {
            case 1:
                DishesNumber = Number[0];
                break;

            case 2:
                DishesNumber = Number[1];
                break;

            case 3:
                DishesNumber = Number[2];
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        switch ((int)this.gameObject.transform.localScale.x)
        {
            case 0:
                DishesNumber = Number[0];
                break;

            case 1:
                DishesNumber = Number[1];
                break;

            case 2:
                DishesNumber = Number[2];
                break;

            default:
                break;
        }
    }
}
