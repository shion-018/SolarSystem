using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    //最大HPと現在のHP。
    int maxtime = 100;
    int currenttime;
    //Sliderを入れる
    public Slider slider;

    int damage = 10;

    float seconds;

    void Start()
    {
        //Sliderを満タンにする。
        slider.value = 1;
        //現在のHPを最大HPと同じに。
        currenttime = maxtime;
        Debug.Log("Start currentHp : " + currenttime);
    }

    //ColliderオブジェクトのIsTriggerにチェック入れること。
    void Update()
    {
        seconds += Time.deltaTime;
        if (seconds >= 1)
        {
            seconds = 0;

            Debug.Log("damage : " + damage);

            //現在のHPからダメージを引く
            currenttime = currenttime - damage;
            Debug.Log("After currentHp : " + currenttime);

            if (currenttime <= 0)
            {
                currenttime = maxtime;
            }

            //最大HPにおける現在のHPをSliderに反映。
            //int同士の割り算は小数点以下は0になるので、
            //(float)をつけてfloatの変数として振舞わせる。
            slider.value = (float)currenttime / (float)maxtime;
        }

    }
}
