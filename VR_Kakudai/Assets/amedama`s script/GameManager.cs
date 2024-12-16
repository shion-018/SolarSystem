
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{

    public static GameManager instance;

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

    public int TimeNow = 0;
    float Seconds_If;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += SceneloadIvent;
    }

    void SceneloadIvent(Scene scene , LoadSceneMode sceneMode)
    {

        TimeLimitText = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();
        Debug.Log(TimeLimitText);

    }

    // Update is called once per frame
    void Update()
    {

        if (gamestate == GAMESTATE.Play)//時間をカウントする処理
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
                    gamestate = GAMESTATE.GameEnd;

                }

            }
        }

    }

}

