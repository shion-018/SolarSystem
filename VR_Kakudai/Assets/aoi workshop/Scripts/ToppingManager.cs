using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ToppingManager : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject[] prefabs; // スポーンさせるPrefabの配列

    [Header("Spawn Positions")]
    public Transform[] spawnPoints; // 各Prefabをスポーンさせる位置の配列

    private List<GameObject> spawnedObjects = new List<GameObject>(); // スポーン済みオブジェクトのリスト
    private Dictionary<GameObject, Vector3> lastPositions = new Dictionary<GameObject, Vector3>(); // 各オブジェクトの最後の位置
    private bool objectsActive = false; // 現在オブジェクトがアクティブかどうか

    private Camera mainCamera; // メインカメラの参照
    private GameObject selectedObject; // 現在選択されているオブジェクト
    private float grabDistance; // オブジェクトを掴む距離
    private Vector3 grabOffset; // 掴んだ際のオフセット位置

    void Start()
    {
        mainCamera = Camera.main; // メインカメラを取得
    }

    void Update()
    {
        // Bボタンまたは右クリックでオブジェクトの表示/非表示を切り替え
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || Input.GetMouseButtonDown(1))
        {
            ToggleObjects();
        }

        HandleMotionDetection(); // オブジェクトの移動検知を処理
        HandleMouseDrag(); // マウスでオブジェクトを掴む処理
    }

    void HandleMotionDetection()
    {
        foreach (GameObject obj in new List<GameObject>(spawnedObjects))
        {
            if (obj == null) continue; // オブジェクトがnullの場合はスキップ

            Vector3 currentPosition = obj.transform.position; // 現在の位置を取得

            // オブジェクトが一定距離以上移動したか確認
            if (lastPositions.ContainsKey(obj) && Vector3.Distance(lastPositions[obj], currentPosition) > 0.01f)
            {
                OnObjectMoved(obj); // 移動イベントを呼び出し
            }

            // 最後の位置を更新
            lastPositions[obj] = currentPosition;
        }
    }

    void HandleMouseDrag()
    {
        // 左クリックでオブジェクトを掴む処理
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // マウス位置からRayを発射
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (spawnedObjects.Contains(hit.collider.gameObject)) // スポーンされたオブジェクトか確認
                {
                    selectedObject = hit.collider.gameObject; // 選択されたオブジェクトをセット
                    grabDistance = Vector3.Distance(mainCamera.transform.position, hit.point); // カメラからの距離を計算
                    grabOffset = hit.collider.transform.position - hit.point; // 掴む位置のオフセットを計算

                    // 物理演算を一時的に無効化
                    Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }
        }

        // 左クリックを押し続けている間、オブジェクトを移動
        if (selectedObject != null && Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 newPosition = ray.GetPoint(grabDistance) + grabOffset; // 新しい位置を計算
            selectedObject.transform.position = newPosition;
        }

        // 左クリックを離したとき、オブジェクトを解放
        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // 物理演算を再有効化
                rb.useGravity = true;
            }
            selectedObject = null; // 選択解除
        }
    }

    void OnObjectMoved(GameObject movedObject)
    {
        Debug.Log($"Object {movedObject.name} has moved!");

        // 物理演算を一時的に無効化
        //Rigidbody rb = movedObject.GetComponent<Rigidbody>();
        //if (rb != null)
        //{
        //    rb.isKinematic = true;
        //    rb.useGravity = false;
        //}

        // リストから削除
        spawnedObjects.Remove(movedObject);

        // 他のオブジェクトを削除
        List<GameObject> objectsToDestroy = new List<GameObject>(spawnedObjects);
        DestroyAllPrefabs(objectsToDestroy);
    }

    void ToggleObjects()
    {
        // オブジェクトがアクティブで、かつ存在する場合は削除
        if (objectsActive && spawnedObjects.Count > 0)
        {
            List<GameObject> objectsToDestroy = new List<GameObject>(spawnedObjects);
            DestroyAllPrefabs(objectsToDestroy);
            objectsActive = false;
        }
        else
        {
            // アクティブでない場合、または削除された場合は新しいオブジェクトをスポーン
            SpawnPrefabs();
            objectsActive = true;
        }
    }

    void SpawnPrefabs()
    {
        // Prefabs と Spawn Points の数が一致しない場合、エラーを表示
        if (prefabs.Length != spawnPoints.Length)
        {
            Debug.LogError("Prefabs と Spawn Points の配列の長さが一致していません！");
            return;
        }

        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] != null && spawnPoints[i] != null)
            {
                // 新しいオブジェクトをスポーン
                GameObject newObject = Instantiate(prefabs[i], spawnPoints[i].position, spawnPoints[i].rotation);
                spawnedObjects.Add(newObject);

                // Collider を確認し、必要なら追加
                Collider collider = newObject.GetComponent<Collider>();
                if (collider == null)
                {
                    collider = newObject.AddComponent<BoxCollider>();
                }
                collider.isTrigger = false;

                // Rigidbody を確認し、必要なら追加
                Rigidbody rb = newObject.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = newObject.AddComponent<Rigidbody>();
                }
                rb.isKinematic = true;
                rb.useGravity = false;

                // 初期位置を記録
                lastPositions[newObject] = newObject.transform.position;
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
                lastPositions.Remove(obj);
                Destroy(obj); // オブジェクトを破棄
            }
        }
    }
}