using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    int maxtime = 100;
    int currenttime;
    public Slider slider;

    int damage = 10;

    bool touch = false;

    float seconds;

    void Start()
    {
        // Sliderコンポーネントを自動的に取得
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }

        // Sliderを満タンにする
        slider.value = 1;
        // 現在の時間を最大時間と同じにする
        currenttime = maxtime;
        Debug.Log("Start currentTime : " + currenttime);
    }

    void Update()
    {
        if (touch)
        {
            seconds += Time.deltaTime;
            if (seconds >= 1)
            {
                seconds = 0;

                Debug.Log("damage : " + damage);

                // 現在の時間からダメージを引く
                currenttime -= damage;
                Debug.Log("After currentTime : " + currenttime);

                if (currenttime <= 0)
                {
                    currenttime = maxtime;
                }

                // 最大時間における現在の時間をSliderに反映
                slider.value = (float)currenttime / (float)maxtime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ばあ");
            touch = true;
        }
    }
}
