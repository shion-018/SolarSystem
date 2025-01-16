using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generation : MonoBehaviour
{
    [SerializeField] private GameObject[] target;

    int i = 0;

    GameObject CustomerCounterObjct, GameManagerObject;
    CustomerCounter CustomerCounter;
    GameManager gameManager;

    //生成間隔（秒）
    [SerializeField] private float spawnInterval = 2.0f;
    private bool isSpawning = false;

    void Start()
    {
        CustomerCounterObjct = GameObject.Find("CustomerCount");
        CustomerCounter = CustomerCounterObjct.GetComponent<CustomerCounter>();

        GameManagerObject = GameObject.Find("GameManager");
        gameManager = GameManagerObject.GetComponent<GameManager>();
    }

    void Update()
    {
        if (CustomerCounter.counter < 3 && !isSpawning)
        {
            gameManager.AllCustomer++;
            StartCoroutine(SpawnWithDelay());
        }
    }

    IEnumerator SpawnWithDelay()
    {
        isSpawning = true;

        //ランダムにオブジェクトを選んで生成
        i = Random.Range(0, target.Length);
        Instantiate(target[i], this.transform.position, Quaternion.identity);

        CustomerCounter.counter++;

        //指定の間隔を待機
        yield return new WaitForSeconds(spawnInterval);

        isSpawning = false;
    }
}
