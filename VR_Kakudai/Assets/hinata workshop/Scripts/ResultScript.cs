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
    [SerializeField] GameObject _comeResult;
    [SerializeField] GameObject _goodResult;
    [SerializeField] GameObject _badResult;
    [SerializeField] GameObject _sumResult;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void EditorResult()
    {
        Text comeResul_text = _comeResult.GetComponent<Text>();
        Text goodResul_text = _goodResult.GetComponent<Text>();
        Text badResul_text = _badResult.GetComponent<Text>();
        Text sumResult_text = _sumResult.GetComponent<Text>();
        comeResul_text.text = "" + comeResult;
        goodResul_text.text = "" + goodResult;
        badResul_text.text = "" + badResult;
        sumResult_text.text = "" + sumResult;
    }
    public void IsGameClear()
    {
        StartCoroutine(RetryAfterDelay(10f)); 
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
}
