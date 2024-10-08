using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject[] objectPrefabs;  // ��������I�u�W�F�N�g�̃v���n�u�z��
    public Transform[] spawnPoints;     // �����|�C���g�̔z��

    private Camera cam;
    private GameObject selectedObject;
    private Rigidbody selectedObjectRb;
    private Vector3 lastMousePosition;
    private bool isGrabbing = false;

    private List<ObjectState> spawnedObjects = new List<ObjectState>(); // �������ꂽ�I�u�W�F�N�g�̏�Ԃ�ǐ�

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleRightClick();  // �E�N���b�N�ŐV�����I�u�W�F�N�g�𐶐��܂��͍폜
        HandleGrab();        // ���N���b�N�Œ͂�
        HandleRelease();     // ���N���b�N�𗣂��ƕ���
        HandleDrag();        // �͂�ł���ԃI�u�W�F�N�g�𓮂���
    }

    // �E�N���b�N�ŃI�u�W�F�N�g�𐶐��܂��͖��I���̂��̂��܂Ƃ߂č폜
    void HandleRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right click detected!");

            // �͂܂�Ă��Ȃ��I�u�W�F�N�g�����݂���ꍇ�A���������ׂč폜
            if (HasUngrabbedObjects())
            {
                DeleteAllUngrabbedObjects();
            }
            else
            {
                // ���ׂč폜���ꂽ�A�܂��͐V�K�����̂Ƃ��͐V�����I�u�W�F�N�g�𐶐�
                SpawnMultipleObjects();
            }
        }
    }

    // �͂܂�Ă��Ȃ��I�u�W�F�N�g�����ׂč폜����
    void DeleteAllUngrabbedObjects()
    {
        List<ObjectState> objectsToRemove = new List<ObjectState>();

        // ���I���̃I�u�W�F�N�g�����ׂč폜�Ώۂɒǉ�
        foreach (var objState in spawnedObjects)
        {
            if (!objState.IsGrabbed && objState.ObjectInstance != null)
            {
                objectsToRemove.Add(objState);
            }
        }

        // ���X�g�̒��̂��ׂĂ̖��I���I�u�W�F�N�g���폜
        foreach (var objState in objectsToRemove)
        {
            Destroy(objState.ObjectInstance);
            Debug.Log("Deleting ungrabbed object: " + objState.ObjectInstance.name);
            objState.ObjectInstance = null;
        }

        // �폜���ꂽ�I�u�W�F�N�g�� spawnedObjects ���X�g����폜
        spawnedObjects.RemoveAll(obj => obj.ObjectInstance == null);
        Debug.Log("All ungrabbed objects deleted.");
    }

    // �͂܂�Ă��Ȃ��I�u�W�F�N�g�����݂��邩�m�F
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

    // �z��Ɋ�Â��ĕ����̃I�u�W�F�N�g�𐶐�����
    void SpawnMultipleObjects()
    {
        if (objectPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No prefabs or spawn points set in the inspector.");
            return;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject prefabToSpawn = objectPrefabs[i % objectPrefabs.Length];  // �v���n�u��I��
            GameObject newObject = Instantiate(prefabToSpawn, spawnPoints[i].position, Quaternion.identity);
            newObject.tag = "Grabbable";  // �^�O��ݒ�
            Rigidbody newObjectRb = newObject.GetComponent<Rigidbody>();
            newObjectRb.useGravity = false; // �������ɂ͏d�͂𖳌���

            // �e�I�u�W�F�N�g�̏�Ԃ�ǐ�
            ObjectState newState = new ObjectState(newObject, spawnPoints[i].position);
            spawnedObjects.Add(newState); // ���X�g�ɒǉ�

            Debug.Log("New object spawned at: " + spawnPoints[i].position);
        }
    }

    // ���N���b�N�ŃI�u�W�F�N�g��͂�
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

                // �͂񂾃I�u�W�F�N�g�̏�Ԃ��X�V
                foreach (var objState in spawnedObjects)
                {
                    if (objState.ObjectInstance == selectedObject)
                    {
                        objState.IsGrabbed = true;
                        break;
                    }
                }

                // ���N���b�N�Œ͂񂾏u�ԁA�͂܂�Ă��Ȃ����ׂẴI�u�W�F�N�g���폜
                DeleteAllUngrabbedObjects();
            }
        }
    }

    // �͂�ł���ԁA�I�u�W�F�N�g�����R�Ɉړ�
    void HandleDrag()
    {
        if (isGrabbing && selectedObject != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 newObjectPos = ray.GetPoint(5); // �J��������̋����𒲐�
            selectedObject.transform.position = newObjectPos;
        }
    }

    // �͂�ł����I�u�W�F�N�g�𗣂�
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

    // �I�u�W�F�N�g�̏�Ԃ��Ǘ�����N���X
    private class ObjectState
    {
        public GameObject ObjectInstance; // �I�u�W�F�N�g�̎���
        public Vector3 InitialPosition;   // �������ꂽ�ʒu
        public bool IsGrabbed;            // �͂܂ꂽ��Ԃ��ǂ���

        public ObjectState(GameObject instance, Vector3 position)
        {
            ObjectInstance = instance;
            InitialPosition = position;
            IsGrabbed = false;
        }
    }
}