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
    private List<GameObject> grabbedObjects = new List<GameObject>(); // 掴まれているオブジェクトを管理するリスト

    void Update()
    {
        // Oculus Quest 2のBボタンまたはマウスの右クリックを検出
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || Input.GetMouseButtonDown(1))
        {
            // 掴まれているオブジェクトを除外してリストをチェック
            List<GameObject> nonGrabbedObjects = new List<GameObject>();

            foreach (GameObject obj in spawnedObjects)
            {
                if (obj != null && !grabbedObjects.Contains(obj))
                {
                    nonGrabbedObjects.Add(obj);
                }
            }

            if (nonGrabbedObjects.Count == 0)
            {
                SpawnPrefabs();
            }
            else
            {
                DestroyAllPrefabs(nonGrabbedObjects);
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

                // 必要に応じて、オブジェクトのコライダーを追加して掴む処理を実装します。
                Collider collider = newObject.GetComponent<Collider>();
                if (collider == null)
                {
                    collider = newObject.AddComponent<BoxCollider>();
                }
                collider.isTrigger = true;

                // Rigidbodyを追加し、物理演算での制御を可能にする
                Rigidbody rb = newObject.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = newObject.AddComponent<Rigidbody>();
                    rb.isKinematic = true; // 初期状態では物理演算をオフにする
                }
            }
        }
    }
    /// <summary>
    /// オブジェクトを掴んだときの処理
    /// </summary>
    public void OnObjectGrabbed(GameObject grabbedObject)
    {
        if (spawnedObjects.Contains(grabbedObject))
        {
            grabbedObjects.Add(grabbedObject);
        }
    }

    /// <summary>
    /// オブジェクトを放したときの処理
    /// </summary>
    public void OnObjectReleased(GameObject releasedObject)
    {
        if (grabbedObjects.Contains(releasedObject))
        {
            grabbedObjects.Remove(releasedObject);

            // Rigidbodyの設定を変更して物理演算を適用するようにする
            Rigidbody rb = releasedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // 物理演算を適用するように設定
            }

            // Colliderの設定を変更して、物理衝突を適用するようにする
            Collider collider = releasedObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = false; // 衝突を有効にする
            }
        }
    }

    /// <summary>
    /// 出現中のPrefabをすべて削除する
    /// </summary>
    void DestroyAllPrefabs(List<GameObject> objectsToDestroy)
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.RemoveAll(obj => objectsToDestroy.Contains(obj)); // 削除済みオブジェクトをリストから削除
    }
}