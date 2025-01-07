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
    private Dictionary<GameObject, Vector3> lastPositions = new Dictionary<GameObject, Vector3>();
    private bool objectsActive = false;

    private Camera mainCamera;
    private GameObject selectedObject;
    private float grabDistance;
    private Vector3 grabOffset;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || Input.GetMouseButtonDown(1))
        {
            ToggleObjects();
        }

        HandleMotionDetection();
        HandleMouseDrag();
    }

    void HandleMotionDetection()
    {
        foreach (GameObject obj in new List<GameObject>(spawnedObjects))
        {
            if (obj == null) continue;

            Vector3 currentPosition = obj.transform.position;

            // Check if the object has moved significantly
            if (lastPositions.ContainsKey(obj) && Vector3.Distance(lastPositions[obj], currentPosition) > 0.01f)
            {
                OnObjectMoved(obj);
            }

            // Update the last known position
            lastPositions[obj] = currentPosition;
        }
    }

    void HandleMouseDrag()
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

                    // Temporarily disable physics
                    Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.useGravity = false;
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
            // Re-enable physics
            Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            selectedObject = null;
        }
    }

    void OnObjectMoved(GameObject movedObject)
    {
        Debug.Log($"Object {movedObject.name} has moved!");

        // Disable physics temporarily
        Rigidbody rb = movedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Remove from active list
        spawnedObjects.Remove(movedObject);

        // Remove other objects
        List<GameObject> objectsToDestroy = new List<GameObject>(spawnedObjects);
        DestroyAllPrefabs(objectsToDestroy);
    }

    void ToggleObjects()
    {
        // Check if there are any objects to destroy
        if (objectsActive && spawnedObjects.Count > 0)
        {
            List<GameObject> objectsToDestroy = new List<GameObject>(spawnedObjects);
            DestroyAllPrefabs(objectsToDestroy);
            objectsActive = false;
        }
        else
        {
            // If no objects are active or there are no objects to destroy, spawn new ones
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

                // Initialize the last position
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
                Destroy(obj);
            }
        }
    }
}
