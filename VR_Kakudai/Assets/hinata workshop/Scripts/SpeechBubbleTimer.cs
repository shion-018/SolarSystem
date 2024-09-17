using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleTimer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float timerLimit;
    float seconds = 0f;

    //追加　ClockをInspector上から設定するため
    [SerializeField] TestClock testclock;

    void Start()
    {

    }

    void Update()
    {
        //変更　ClockのUpdateClock関数を呼び出す
        //　　　引数は_updateTimer()のtimerの値
        testclock.UpdateClock(_updateTimer());
    }

    //変更　void からfloat
    float _updateTimer()
    {
        seconds += Time.deltaTime;
        float timer = seconds / timerLimit;

        //追加　float型のtimerを返す
        return timer;
    }
}
