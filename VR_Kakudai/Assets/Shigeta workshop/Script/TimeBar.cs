using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
   
    public int maxtime;
    int currenttime;
    public Slider slider;

    int damage = 1;

    bool touch = false;

    float seconds;

    void Start()
    {
        maxtime = Random.Range(6, 10) * 10;


        //Sliderコンポーネントを自動的に取得
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }

        //Sliderを満タンにする
        slider.value = 1;
        //現在の時間を最大時間と同じにする
        currenttime = maxtime;
        
    }

    void Update()
    {
        if (touch)
        {
            seconds += Time.deltaTime;
            if (seconds >= 1)
            {
                seconds = 0;

                

                //現在の時間からダメージを引く
                currenttime -= damage;

                if (currenttime <= 0)
                {
                    Debug.Log("時間切れ");
                   
                }

                //最大時間における現在の時間をSliderに反映
                slider.value = (float)currenttime / (float)maxtime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("ばあ");
            touch = true;
        }

        if(maxtime != currenttime)
        {
            currenttime = maxtime;
        }
    }
}
