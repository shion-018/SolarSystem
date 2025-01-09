using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScript : MonoBehaviour
{
    int comeResult;
    int goodResult;
    int badResult;
    int sumResult;
    [Header("リザルトの数字")]
    [SerializeField] GameObject resultCanvas;
    [SerializeField] GameObject _comeResult;
    [SerializeField] GameObject _goodResult;
    [SerializeField] GameObject _badResult;
    [Header("リザルトの画像")]
    [SerializeField] GameObject god;
    [SerializeField] GameObject excerent;
    [SerializeField] GameObject good;
    [SerializeField] GameObject close;

    // Start is called before the first frame update
    void Start()
    {
        resultCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void EditorResult()
    {
        resultCanvas.SetActive(true);
        Text comeResul_text = _comeResult.GetComponent<Text>();
        Text goodResul_text = _goodResult.GetComponent<Text>();
        Text badResul_text = _badResult.GetComponent<Text>();
        comeResul_text.text = "" + comeResult;
        goodResul_text.text = "" + goodResult;
        badResul_text.text = "" + badResult;
        IsGameClear();
        Result();
    }
    public void IsGameClear()
    {
        StartCoroutine(RetryAfterDelay(10f)); //10秒後にシーンをリロード
    }
    private IEnumerator RetryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        RetryGame(); 
    }
    void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Result()
    {
        //点数の判別欄
        sumResult = goodResult*3 + badResult;

        if (sumResult >= 15)
        {
            god.SetActive(true);
        }
        if (sumResult >= 10 &&  sumResult < 15)
        {
            excerent.SetActive(true);
        }
        if(sumResult >= 5 && sumResult < 10)
        {
            good.SetActive(true);
        }
        if( sumResult < 5)
        {
            close.SetActive(true);
        }
    }
}
