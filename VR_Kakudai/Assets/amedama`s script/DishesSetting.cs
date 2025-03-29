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

    /*[HideInInspector]*/ public int DishesNumber = 999;
    //[SerializeField] public float[,] DishesMagnificationPrice;//Šg‘å‚µ‚½Žž‚ÌA’iŠK‚Æ”{—¦‚ðŒˆ‚ß‚Ä‚Ù‚µ‚¢‚Å‚·B


    public DishesMagnification[] dishesMagnifications = new DishesMagnification[3];
    [System.Serializable] public class DishesMagnification { public float[] PriceMagnification = new float[2]{1,1}; }

    private float CriterionSize;
    // Start is called before the first frame update
    void Start()
    {

        CriterionSize = this.gameObject.transform.localScale.x;

        if(this.gameObject.transform.localScale.x >= CriterionSize * 3)
        {
            DishesNumber = Number[2];
        }
        else if(this.gameObject.transform.localScale.x >= CriterionSize * 2)
        {
            DishesNumber = Number[1];
        }
        else if(this.gameObject.transform.localScale.x >= CriterionSize)
        {
            DishesNumber = Number[0];
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (this.gameObject.transform.localScale.x >= CriterionSize * 2)
        {
            DishesNumber = Number[2];
        }
        else if (this.gameObject.transform.localScale.x >= CriterionSize)
        {
            DishesNumber = Number[1];
        }
        else if (this.gameObject.transform.localScale.x < CriterionSize)
        {
            DishesNumber = Number[0];
        }

    }
}
