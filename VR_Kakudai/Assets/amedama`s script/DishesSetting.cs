using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishesSetting : MonoBehaviour   
{

   // [SerializeField] public GameObject Dishes;
    [SerializeField] public Sprite DishesSprite;
    [SerializeField] public int DishesPrice = 700;
    //[SerializeField] public float[,] DishesMagnificationPrice;//拡大した時の、段階と倍率を決めてほしいです。
    
    
    public DishesMagnification[] dishesMagnifications = new DishesMagnification[3];
    [System.Serializable] public class DishesMagnification { public float[] PriceMagnification = new float[2]{1,1}; }


    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
