using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Oculus.Interaction;

public class ToppingManager : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject[] prefabs;

    [Header("Spawn Positions")]
    public Transform[] spawnPoints;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Dictionary<GameObject, Vector3> lastPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, bool> gravityEnabled = new Dictionary<GameObject, bool>();
    private bool objectsActive = false;

    private Camera mainCamera;
    private GameObject selectedObject;
    private float grabDistance;
    private Vector3 grabOffset;

    void Start()
    {
        mainCamera = Camera.main;
        SetupGrabListeners();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || Input.GetMouseButtonDown(1))
        {
            ToggleObjects();
        }

        HandleMotionDetection();
        HandleMouseGrab();
    }

    void HandleMouseGrab()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (spawnedObjects.Contains(hit.collider.gameObject))
                {
                    selectedObject = hit.collider.gameObject;
                    grabDistance = Vector3.Distance(mainCamera.transform.position, hit.point);
                    grabOffset = hit.collider.transform.position - hit.point;

                    // オブジェクトごとの重力制御
                    if (!gravityEnabled.GetValueOrDefault(selectedObject, false))
                    {
                        EnableGravity(selectedObject);
                        gravityEnabled[selectedObject] = true;
                    }

                    Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }
        }

        if (selectedObject != null && Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 newPosition = ray.GetPoint(grabDistance) + grabOffset;
            selectedObject.transform.position = newPosition;
        }

        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            selectedObject = null;
        }
    }

    void SetupGrabListeners()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
            {
                SetupGrabListener(obj);
            }
        }
    }

    void SetupGrabListener(GameObject obj)
    {
        var grabbable = obj.GetComponent<Grabbable>();
        if (grabbable != null)
        {
            // SelectとUnselectの両方のイベントを監視
            grabbable.WhenPointerEventRaised += (evt) =>
            {
                switch (evt.Type)
                {
                    case PointerEventType.Select:
                        if (!gravityEnabled.GetValueOrDefault(obj, false))
                        {
                            EnableGravity(obj);
                            gravityEnabled[obj] = true;
                        }
                        // 掴んでいる間はKinematicに
                        var rb = obj.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = true;
                        }
                        break;

                    case PointerEventType.Unselect:
                        // 離した時にKinematicを解除
                        rb = obj.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = false;
                        }
                        break;
                }
            };
        }
    }

    void EnableGravity(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            Debug.Log($"Gravity enabled for {obj.name}");
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
                gravityEnabled[newObject] = false;

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
                rb.useGravity = false;
                rb.isKinematic = true;

                var grabbable = newObject.GetComponent<Grabbable>();
                if (grabbable == null)
                {
                    Debug.LogWarning($"Grabbableコンポーネントが{newObject.name}に見つかりませんでした。掴む機能が動作しない可能性があります。");
                }
                else
                {
                    SetupGrabListener(newObject);
                }

                lastPositions[newObject] = newObject.transform.position;
            }
        }
    }

    void HandleMotionDetection()
    {
        foreach (GameObject obj in new List<GameObject>(spawnedObjects))
        {
            if (obj == null) continue;

            Vector3 currentPosition = obj.transform.position;

            if (lastPositions.ContainsKey(obj) && Vector3.Distance(lastPositions[obj], currentPosition) > 0.01f)
            {
                OnObjectMoved(obj);
            }

            lastPositions[obj] = currentPosition;
        }
    }

    void OnObjectMoved(GameObject movedObject)
    {
        Debug.Log($"Object {movedObject.name} has moved!");
        spawnedObjects.Remove(movedObject);

        List<GameObject> objectsToDestroy = new List<GameObject>(spawnedObjects);
        DestroyAllPrefabs(objectsToDestroy);
    }

    void ToggleObjects()
    {
        if (objectsActive && spawnedObjects.Count > 0)
        {
            List<GameObject> objectsToDestroy = new List<GameObject>(spawnedObjects);
            DestroyAllPrefabs(objectsToDestroy);
            objectsActive = false;
        }
        else
        {
            SpawnPrefabs();
            objectsActive = true;
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
                gravityEnabled.Remove(obj);
                Destroy(obj);
            }
        }
    }
}