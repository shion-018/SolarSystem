using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ToppingManager : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject[] prefabs; // 出現させるPrefabの配列

    [Header("Spawn Positions")]
    public Transform[] spawnPoints; // 配置位置を格納する配列

    private List<GameObject> spawnedObjects = new List<GameObject>(); // 出現中のオブジェクトを管理するリスト

    void Update()
    {
        // Oculus Quest 2のBボタンまたはマウスの右クリックを検出
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) || Input.GetMouseButtonDown(1))
        {
            if (spawnedObjects.Count == 0)
            {
                SpawnPrefabs();
            }
            else
            {
                DestroyAllPrefabs();
            }
        }
    }

    /// <summary>
    /// Prefabsを出現させる
    /// </summary>
    void SpawnPrefabs()
    {
        if (prefabs.Length != spawnPoints.Length)
        {
            Debug.LogError("Prefabs と Spawn Points の配列の長さが一致していません！");
            return;
        }

        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] != null && spawnPoints[i] != null)
            {
                GameObject newObject = Instantiate(prefabs[i], spawnPoints[i].position, spawnPoints[i].rotation);
                spawnedObjects.Add(newObject); // リストに追加
            }
        }
    }

    /// <summary>
    /// 出現中のPrefabをすべて削除する
    /// </summary>
    void DestroyAllPrefabs()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear(); // リストをクリア
    }
}