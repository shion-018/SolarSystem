using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ToppingManager : MonoBehaviour
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
        HandleRightClick();  // 右クリック（VRではBボタン）で新しいオブジェクトを生成または削除
        HandleGrab();        // 左クリック（VRではRTボタン）で掴む
        HandleRelease();     // 左クリックを離すと放つ
        HandleDrag();        // 掴んでいる間オブジェクトを動かす
    }

    // 右クリックでオブジェクトを生成または未選択のものをまとめて削除
    void HandleRightClick()
    {
        bool rightClick = Input.GetMouseButtonDown(1) || OVRInput.GetDown(OVRInput.RawButton.B);

        // 左クリックが長押しされている間は新規オブジェクトを生成しない
        if (rightClick && !isGrabbing)
        {
            Debug.Log("Right click detected!");

            if (HasUngrabbedObjects())
            {
                DeleteAllUngrabbedObjects();
            }
            else
            {
                SpawnMultipleObjects();
            }
        }
    }

    // 未選択のオブジェクトが存在するか確認
    bool HasUngrabbedObjects()
    {
        // リストからすでに消滅したオブジェクトを削除
        spawnedObjects.RemoveAll(obj => obj.ObjectInstance == null);

        // リスト内に未選択のオブジェクトが残っているかを確認
        foreach (var objState in spawnedObjects)
        {
            if (!objState.IsGrabbed)
            {
                return true;
            }
        }
        return false;
    }

    // 未選択のオブジェクトを全て削除
    void DeleteAllUngrabbedObjects()
    {
        List<ObjectState> objectsToRemove = new List<ObjectState>();

        foreach (var objState in spawnedObjects)
        {
            // オブジェクトが存在し、かつ未選択の場合、削除対象に追加
            if (!objState.IsGrabbed && objState.ObjectInstance != null)
            {
                objectsToRemove.Add(objState);
            }
        }

        // 削除対象のオブジェクトを消去
        foreach (var objState in objectsToRemove)
        {
            Destroy(objState.ObjectInstance);
            objState.ObjectInstance = null;
        }

        // すでに消滅したオブジェクトもリストから削除
        spawnedObjects.RemoveAll(obj => obj.ObjectInstance == null);
        Debug.Log("All ungrabbed objects deleted.");
    }

    // 複数のオブジェクトを生成
    void SpawnMultipleObjects()
    {
        if (objectPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No prefabs or spawn points set in the inspector.");
            return;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject prefabToSpawn = objectPrefabs[i % objectPrefabs.Length];
            GameObject newObject = Instantiate(prefabToSpawn, spawnPoints[i].position, Quaternion.identity);
            newObject.tag = "sushi";
            Rigidbody newObjectRb = newObject.GetComponent<Rigidbody>();
            newObjectRb.useGravity = false;

            ObjectState newState = new ObjectState(newObject, spawnPoints[i].position);
            spawnedObjects.Add(newState);

            Debug.Log("New object spawned at: " + spawnPoints[i].position);
        }
    }

    // オブジェクトを掴む
    void HandleGrab()
    {
        bool leftClick = Input.GetMouseButtonDown(0) /*|| Input.GetAxis("XRI_Right_Trigger") > 0.5f*/;

        if (leftClick && !isGrabbing)
        {
            Debug.Log("Left click detected!");

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("sushi"))
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

    // 掴んでいる間オブジェクトをドラッグする
    void HandleDrag()
    {
        if (isGrabbing && selectedObject != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 newObjectPos = ray.GetPoint(5);
            selectedObject.transform.position = newObjectPos;
        }
    }

    // 掴んでいるオブジェクトを放す
    void HandleRelease()
    {
        bool leftRelease = Input.GetMouseButtonUp(0) /*|| Input.GetAxis("XRI_Right_Trigger") < 0.5f*/;

        if (isGrabbing && leftRelease)
        {
            Debug.Log("Left click release detected!");

            // 重力を有効にして、オブジェクトが自然に落ちるようにする
            selectedObjectRb.isKinematic = false;
            selectedObjectRb.useGravity = true;

            isGrabbing = false;
            selectedObject = null;
            selectedObjectRb = null;
        }
    }

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
}