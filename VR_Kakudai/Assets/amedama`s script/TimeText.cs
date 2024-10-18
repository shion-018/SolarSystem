using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI TimeLimitText;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameManager.TimeNow);
        TimeLimitText.text = gameManager.TimeNow.ToString();
    }
}
