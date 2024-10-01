using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;



public class GameManager : MonoBehaviour
{

    [SerializeField, Tooltip("タイムリミット（秒）")] int TimeLimit = 60;
    [SerializeField, Tooltip("制限時間Text")] TextMeshProUGUI TimeLimitText;
    [SerializeField, Tooltip("")]

    public enum GAMESTATE
    {
        Title,
        Movie,
        Play,
        Pose,
        GameClear,
        GameEnd,
    }

    public GAMESTATE gamestate = GAMESTATE.Title;

    int TimeNow = 0;
    float Seconds_If;

    // Start is called before the first frame update
    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKey(KeyCode.Space))
        {
            gamestate = GAMESTATE.Play;
        }

        if (gamestate == GAMESTATE.Play)
        {
            Seconds_If += Time.deltaTime;

            if (Seconds_If >= 1.0f)
            {
                TimeNow += 1;
                TimeLimitText.text = TimeNow.ToString();
                Seconds_If = 0;

                if (TimeLimit <= TimeNow)
                {
                    TimeNow = 0;

                }

            }
        }

    }

    


}
