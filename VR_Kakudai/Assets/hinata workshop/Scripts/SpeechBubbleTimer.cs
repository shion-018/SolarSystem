using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubbleTimer : MonoBehaviour
{
    //timerLimitÇ≈êßå¿éûä‘ïœçX
    [SerializeField] float timerLimit;
    float seconds = 0f;
    
    [SerializeField] TestClock testclock;
    [SerializeField] Image hukidasi;
    void Start()
    {
        hukidasi.color = Color.green;
    }

    void Update()
    {
        testclock.UpdateClock(_updateTimer());
    }
    float _updateTimer()
    {
        seconds += Time.deltaTime;
        float timer = 1 - seconds / timerLimit;
        if(timer < 0.25)
        {
            hukidasi.color = Color.red;
        }

        return timer;
    }
}
