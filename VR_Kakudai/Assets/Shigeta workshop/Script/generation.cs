using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generation : MonoBehaviour
{
    [SerializeField] private GameObject[] target;

    int i = 0;

    GameObject A;
    CustomerCounter CustmerCounter;

    // 生成間隔（秒）
    [SerializeField] private float spawnInterval = 2.0f;
    private bool isSpawning = false;

    void Start()
    {
        A = GameObject.Find("CustonerCount");
        CustmerCounter = A.GetComponent<CustomerCounter>();
    }

    void Update()
    {
        if (CustmerCounter.counter < 3 && !isSpawning)
        {
            StartCoroutine(SpawnWithDelay());
        }
    }

    IEnumerator SpawnWithDelay()
    {
        isSpawning = true;  // 生成中フラグをオン

        // ランダムにオブジェクトを選んで生成
        i = Random.Range(0, target.Length);
        Instantiate(target[i], this.transform.position, Quaternion.identity);

        // カウンターを増やす
        CustmerCounter.counter++;

        // 指定の間隔を待機
        yield return new WaitForSeconds(spawnInterval);

        isSpawning = false;  // 生成中フラグをオフ
    }
}
