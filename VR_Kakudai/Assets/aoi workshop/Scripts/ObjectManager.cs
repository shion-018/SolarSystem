using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject[] objectPrefabs;  // 生成するオブジェクトのプレハブ配列
    public Transform[] spawnPoints;     // 生成ポイントの配列

    private Camera cam;
    private GameObject selectedObject;
    private Rigidbody selectedObjectRb;
    private Vector3 lastMousePosition;
    private bool isGrabbing = false;

    private List<ObjectState> spawnedObjects = new List<ObjectState>(); // 生成されたオブジェクトの状態を追跡

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleRightClick();  // 右クリックで新しいオブジェクトを生成または削除
        HandleGrab();        // 左クリックで掴む
        HandleRelease();     // 左クリックを離すと放つ
        HandleDrag();        // 掴んでいる間オブジェクトを動かす
    }

    // 右クリックでオブジェクトを生成または未選択のものをまとめて削除
    void HandleRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right click detected!");

            // 掴まれていないオブジェクトが存在する場合、それらをすべて削除
            if (HasUngrabbedObjects())
            {
                DeleteAllUngrabbedObjects();
            }
            else
            {
                // すべて削除された、または新規生成のときは新しいオブジェクトを生成
                SpawnMultipleObjects();
            }
        }
    }

    // 掴まれていないオブジェクトをすべて削除する
    void DeleteAllUngrabbedObjects()
    {
        List<ObjectState> objectsToRemove = new List<ObjectState>();

        // 未選択のオブジェクトをすべて削除対象に追加
        foreach (var objState in spawnedObjects)
        {
            if (!objState.IsGrabbed && objState.ObjectInstance != null)
            {
                objectsToRemove.Add(objState);
            }
        }

        // リストの中のすべての未選択オブジェクトを削除
        foreach (var objState in objectsToRemove)
        {
            Destroy(objState.ObjectInstance);
            Debug.Log("Deleting ungrabbed object: " + objState.ObjectInstance.name);
            objState.ObjectInstance = null;
        }

        // 削除されたオブジェクトを spawnedObjects リストから削除
        spawnedObjects.RemoveAll(obj => obj.ObjectInstance == null);
        Debug.Log("All ungrabbed objects deleted.");
    }

    // 掴まれていないオブジェクトが存在するか確認
    bool HasUngrabbedObjects()
    {
        foreach (var objState in spawnedObjects)
        {
            if (!objState.IsGrabbed)
            {
                return true;
            }
        }
        return false;
    }

    // 配列に基づいて複数のオブジェクトを生成する
    void SpawnMultipleObjects()
    {
        if (objectPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No prefabs or spawn points set in the inspector.");
            return;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject prefabToSpawn = objectPrefabs[i % objectPrefabs.Length];  // プレハブを選択
            GameObject newObject = Instantiate(prefabToSpawn, spawnPoints[i].position, Quaternion.identity);
            newObject.tag = "Grabbable";  // タグを設定
            Rigidbody newObjectRb = newObject.GetComponent<Rigidbody>();
            newObjectRb.useGravity = false; // 生成時には重力を無効に

            // 各オブジェクトの状態を追跡
            ObjectState newState = new ObjectState(newObject, spawnPoints[i].position);
            spawnedObjects.Add(newState); // リストに追加

            Debug.Log("New object spawned at: " + spawnPoints[i].position);
        }
    }

    // 左クリックでオブジェクトを掴む
    void HandleGrab()
    {
        if (Input.GetMouseButtonDown(0) && !isGrabbing)
        {
            Debug.Log("Left click detected!");

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Grabbable"))
            {
                selectedObject = hit.collider.gameObject;
                selectedObjectRb = selectedObject.GetComponent<Rigidbody>();
                selectedObjectRb.isKinematic = true;
                selectedObjectRb.useGravity = false;
                lastMousePosition = Input.mousePosition;
                isGrabbing = true;

                // 掴んだオブジェクトの状態を更新
                foreach (var objState in spawnedObjects)
                {
                    if (objState.ObjectInstance == selectedObject)
                    {
                        objState.IsGrabbed = true;
                        break;
                    }
                }

                // 左クリックで掴んだ瞬間、掴まれていないすべてのオブジェクトを削除
                DeleteAllUngrabbedObjects();
            }
        }
    }

    // 掴んでいる間、オブジェクトを自由に移動
    void HandleDrag()
    {
        if (isGrabbing && selectedObject != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 newObjectPos = ray.GetPoint(5); // カメラからの距離を調整
            selectedObject.transform.position = newObjectPos;
        }
    }

    // 掴んでいたオブジェクトを離す
    void HandleRelease()
    {
        if (isGrabbing && Input.GetMouseButtonUp(0))
        {
            Debug.Log("Left click release detected!");

            selectedObjectRb.isKinematic = false;
            selectedObjectRb.useGravity = true;

            isGrabbing = false;
            selectedObject = null;
            selectedObjectRb = null;
        }
    }

    // オブジェクトの状態を管理するクラス
    private class ObjectState
    {
        public GameObject ObjectInstance; // オブジェクトの実体
        public Vector3 InitialPosition;   // 生成された位置
        public bool IsGrabbed;            // 掴まれた状態かどうか

        public ObjectState(GameObject instance, Vector3 position)
        {
            ObjectInstance = instance;
            InitialPosition = position;
            IsGrabbed = false;
        }
    }
}