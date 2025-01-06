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

    private GameObject grabbedObject; // 左クリックで掴むオブジェクト
    private Camera mainCamera; // メインカメラ

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Oculus Quest 2のBボタンまたはマウスの右クリックを検出
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || Input.GetMouseButtonDown(1))
        {
            HandleSpawnOrDestroy();
        }

        // 左クリックでオブジェクトを掴む
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabObject();
        }

        // 左クリックを離したらオブジェクトを放す
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
        }
    }

    /// <summary>
    /// オブジェクトの生成または削除を命ずる
    /// </summary>
    void HandleSpawnOrDestroy()
    {
        // 掴まれていないオブジェクトを検索
        List<GameObject> nonGrabbedObjects = new List<GameObject>();
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null && !grabbedObjects.Contains(obj))
            {
                nonGrabbedObjects.Add(obj);
            }
        }

        // 掴まれていないオブジェクトがない場合は新しいPrefabを生成
        if (nonGrabbedObjects.Count == 0)
        {
            SpawnPrefabs();
        }
        else
        {
            // 掴まれていないオブジェクトを削除
            DestroyAllPrefabs(nonGrabbedObjects);
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

                // Colliderを追加
                Collider collider = newObject.GetComponent<Collider>();
                if (collider == null)
                {
                    collider = newObject.AddComponent<BoxCollider>();
                }
                collider.isTrigger = false;

                // Rigidbodyを追加
                Rigidbody rb = newObject.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = newObject.AddComponent<Rigidbody>();
                }
                rb.isKinematic = true; // 初期状態で物理演算を無効化
                rb.useGravity = false; // 初期状態で重力を無効化
            }
        }
    }

    /// <summary>
    /// 左クリックでオブジェクトを掴む
    /// </summary>
    void TryGrabObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (spawnedObjects.Contains(hitObject) && !grabbedObjects.Contains(hitObject))
            {
                grabbedObject = hitObject;
                grabbedObjects.Add(hitObject);

                // 他のオブジェクトを消去
                DestroyAllPrefabs(new List<GameObject>(spawnedObjects.FindAll(obj => obj != hitObject && !grabbedObjects.Contains(obj))));

                // 重力と物理演算を無効化
                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
            }
        }
    }

    /// <summary>
    /// 左クリックを離したときの処理
    /// </summary>
    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // 物理演算を有効化
                rb.useGravity = true; // 重力有効化
            }
            grabbedObject = null; // リファレンスを解除
        }
    }

    /// <summary>
    /// 出現中のPrefabを削除
    /// </summary>
    void DestroyAllPrefabs(List<GameObject> objectsToDestroy)
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
            {
                spawnedObjects.Remove(obj);
                Destroy(obj);
            }
        }
    }
}