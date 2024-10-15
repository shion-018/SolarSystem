using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingManager : MonoBehaviour
{
    public GameObject[] objectPrefabs;  // 生成するオブジェクトのプレハブ配列
    public Transform[] spawnPoints;     // 生成ポイントの配列

    private Camera cam;
    private GameObject selectedObject;
    private Rigidbody selectedObjectRb;
    private Vector3 lastMousePosition;
    private bool isGrabbing = false;
    private bool isRecentlyReleased = false;  // 掴んだ直後かを示すフラグ

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
        if (isGrabbing)
        {
            return;  // 掴んでいる場合は処理をスキップ
        }

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
                // 新しいオブジェクトを生成
                SpawnMultipleObjects();
            }
        }
    }

    // 掴まれていないオブジェクトをすべて削除する
    void DeleteAllUngrabbedObjects()
    {
        List<ObjectState> objectsToRemove = new List<ObjectState>();

        foreach (var objState in spawnedObjects)
        {
            if (!objState.IsGrabbed && objState.ObjectInstance != null)
            {
                objectsToRemove.Add(objState);
            }
        }

        foreach (var objState in objectsToRemove)
        {
            Destroy(objState.ObjectInstance);
            objState.ObjectInstance = null;
        }

        spawnedObjects.RemoveAll(obj => obj.ObjectInstance == null);
    }

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

    // 複数のオブジェクトを生成する
    void SpawnMultipleObjects()
    {
        if (objectPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No prefabs or spawn points set.");
            return;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject prefabToSpawn = objectPrefabs[i % objectPrefabs.Length];
            GameObject newObject = Instantiate(prefabToSpawn, spawnPoints[i].position, Quaternion.identity);
            newObject.tag = "Grabbable";
            Rigidbody newObjectRb = newObject.GetComponent<Rigidbody>();
            newObjectRb.useGravity = false;
            newObjectRb.isKinematic = true;  // 生成時に物理挙動を無効化

            ObjectState newState = new ObjectState(newObject, spawnPoints[i].position);
            spawnedObjects.Add(newState);
        }
    }

    // 左クリックでオブジェクトを掴む
    void HandleGrab()
    {
        if (Input.GetMouseButtonDown(0) && !isGrabbing)
        {
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

                foreach (var objState in spawnedObjects)
                {
                    if (objState.ObjectInstance == selectedObject)
                    {
                        objState.IsGrabbed = true;
                        break;
                    }
                }

                DeleteAllUngrabbedObjects();
            }
        }
    }

    // 掴んでいる間オブジェクトを移動
    void HandleDrag()
    {
        if (isGrabbing && selectedObject != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 newObjectPos = ray.GetPoint(5);
            selectedObject.transform.position = newObjectPos;
        }
    }

    // 掴んでいたオブジェクトを離す
    void HandleRelease()
    {
        if (isGrabbing && Input.GetMouseButtonUp(0))
        {
            selectedObjectRb.isKinematic = false;
            selectedObjectRb.useGravity = true;

            isGrabbing = false;
            isRecentlyReleased = true;  // 掴んだ直後のフラグを立てる

            selectedObject = null;
            selectedObjectRb = null;
        }
    }

    // オブジェクトの状態を管理するクラス
    private class ObjectState
    {
        public GameObject ObjectInstance;
        public Vector3 InitialPosition;
        public bool IsGrabbed;

        public ObjectState(GameObject instance, Vector3 position)
        {
            ObjectInstance = instance;
            InitialPosition = position;
            IsGrabbed = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 落下させたオブジェクトのみ移動させる
        if (isRecentlyReleased && collision.gameObject.CompareTag("Grabbable"))
        {
            Rigidbody collidedRb = collision.gameObject.GetComponent<Rigidbody>();
            if (collidedRb != null && !collidedRb.isKinematic)
            {
                // 衝突したオブジェクトが掴んだオブジェクトなら移動させる
                collidedRb.isKinematic = false;
                isRecentlyReleased = false;  // フラグリセット
            }
        }
    }
}