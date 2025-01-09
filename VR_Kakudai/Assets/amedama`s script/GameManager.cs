using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField, Tooltip("タイムリミット（秒）")] int TimeLimit = 60;
    [SerializeField, Tooltip("制限時間Text")] TextMeshProUGUI TimeLimitText;
    [SerializeField, Tooltip("")]GameObject ResultUI;
    [SerializeField] ResultScript ResultScript;
    public  bool betogether = false;
    public int AllCustomer = 0;
    public int SatisfiedCustomers = 0;


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
    bool flag = true;


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
        TimeNow = TimeLimit;
        ResultUI.SetActive(false);
        ResultScript = ResultUI.GetComponent<ResultScript>();
    }

    void SceneloadIvent(Scene scene , LoadSceneMode sceneMode)
    {

        TimeLimitText = GameObject.Find("TimeText").GetComponent<TextMeshProUGUI>();
        ResultUI = GameObject.Find("ResultUI");

        Debug.Log(ResultUI);

    }

    // Update is called once per frame
    void Update()
    {

        if (gamestate == GAMESTATE.Play)//時間をカウントする処理
        {
            Seconds_If += Time.deltaTime;

            if (Seconds_If >= 1.0f)
            {
                TimeNow -= 1;
                TimeLimitText.text = TimeNow.ToString();
                Seconds_If = 0;

                if (TimeNow <= 0)
                {
                    TimeNow = 0;
                    gamestate = GAMESTATE.GameEnd;

                }

            }
        }

        if (gamestate == GAMESTATE.GameEnd)//ゲームが終了したとき
        {
            if (flag)
            {
                ResultUI.SetActive(true);
                ResultScript.comeResult = AllCustomer;
                ResultScript.goodResult = SatisfiedCustomers;
                ResultScript.badResult = AllCustomer - SatisfiedCustomers;
                ResultScript.EditorResult();
                
                flag = false;
            }

        }
    }
}

