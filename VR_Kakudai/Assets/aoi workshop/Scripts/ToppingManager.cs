using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ToppingManager : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject[] prefabs;

    [Header("Spawn Positions")]
    public Transform[] spawnPoints;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool objectsActive = false;

    private GameObject grabbedObject;
    private float grabDistance;
    private Vector3 grabOffset;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch)) // Bボタン
        {
            ToggleObjects();
        }

        HandleVRControl();
    }

    void HandleVRControl()
    {
        // 掴む処理
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)) // RTトリガー
        {
            Ray ray = new Ray(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (spawnedObjects.Contains(hit.collider.gameObject))
                {
                    grabbedObject = hit.collider.gameObject;
                    grabDistance = Vector3.Distance(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), hit.point);
                    grabOffset = hit.collider.transform.position - hit.point;

                    // 物理演算を一時的に無効化
                    Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.useGravity = false;
                    }

                    // 掴んだオブジェクトを管理対象から外す
                    spawnedObjects.Remove(grabbedObject);

                    // 他のオブジェクトを全て削除
                    List<GameObject> objectsToDestroy = new List<GameObject>(spawnedObjects);
                    DestroyAllPrefabs(objectsToDestroy);

                    // オブジェクトが空の場合、新しい出現を許可
                    if (spawnedObjects.Count == 0)
                    {
                        objectsActive = false;
                    }
                }
            }
        }

        // 掴んだオブジェクトを動かす
        if (grabbedObject != null && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)) // RTトリガーを押し続けている間
        {
            Ray ray = new Ray(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward);
            Vector3 newPosition = ray.GetPoint(grabDistance) + grabOffset;
            grabbedObject.transform.position = newPosition;
        }

        // 掴んだオブジェクトを解放
        if (grabbedObject != null && OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)) // RTトリガーを離す
        {
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            grabbedObject = null;
        }
    }

    void ToggleObjects()
    {
        if (objectsActive)
        {
            // 現在のオブジェクトを全て削除
            List<GameObject> objectsToDestroy = new List<GameObject>(spawnedObjects);
            if (objectsToDestroy.Count > 0)
            {
                DestroyAllPrefabs(objectsToDestroy);
                objectsActive = false;
            }
            else
            {
                // 削除対象がない場合は次回のBボタンでオブジェクトを生成
                objectsActive = false;
            }
        }
        else
        {
            SpawnPrefabs();
            objectsActive = true;
        }
    }

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
                spawnedObjects.Add(newObject);

                Collider collider = newObject.GetComponent<Collider>();
                if (collider == null)
                {
                    collider = newObject.AddComponent<BoxCollider>();
                }
                collider.isTrigger = false;

                Rigidbody rb = newObject.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = newObject.AddComponent<Rigidbody>();
                }
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }

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