using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingManager : MonoBehaviour
{
    public GameObject[] objectPrefabs;  // ��������I�u�W�F�N�g�̃v���n�u�z��
    public Transform[] spawnPoints;     // �����|�C���g�̔z��

    private Camera cam;
    private GameObject selectedObject;
    private Rigidbody selectedObjectRb;
    private Vector3 lastMousePosition;
    private bool isGrabbing = false;
    private bool isRecentlyReleased = false;  // �͂񂾒��ォ�������t���O

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
        if (isGrabbing)
        {
            return;  // �͂�ł���ꍇ�͏������X�L�b�v
        }

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
                // �V�����I�u�W�F�N�g�𐶐�
                SpawnMultipleObjects();
            }
        }
    }

    // �͂܂�Ă��Ȃ��I�u�W�F�N�g�����ׂč폜����
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

    // �����̃I�u�W�F�N�g�𐶐�����
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
            newObjectRb.isKinematic = true;  // �������ɕ��������𖳌���

            ObjectState newState = new ObjectState(newObject, spawnPoints[i].position);
            spawnedObjects.Add(newState);
        }
    }

    // ���N���b�N�ŃI�u�W�F�N�g��͂�
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

    // �͂�ł���ԃI�u�W�F�N�g���ړ�
    void HandleDrag()
    {
        if (isGrabbing && selectedObject != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 newObjectPos = ray.GetPoint(5);
            selectedObject.transform.position = newObjectPos;
        }
    }

    // �͂�ł����I�u�W�F�N�g�𗣂�
    void HandleRelease()
    {
        if (isGrabbing && Input.GetMouseButtonUp(0))
        {
            selectedObjectRb.isKinematic = false;
            selectedObjectRb.useGravity = true;

            isGrabbing = false;
            isRecentlyReleased = true;  // �͂񂾒���̃t���O�𗧂Ă�

            selectedObject = null;
            selectedObjectRb = null;
        }
    }

    // �I�u�W�F�N�g�̏�Ԃ��Ǘ�����N���X
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
        // �����������I�u�W�F�N�g�݈̂ړ�������
        if (isRecentlyReleased && collision.gameObject.CompareTag("Grabbable"))
        {
            Rigidbody collidedRb = collision.gameObject.GetComponent<Rigidbody>();
            if (collidedRb != null && !collidedRb.isKinematic)
            {
                // �Փ˂����I�u�W�F�N�g���͂񂾃I�u�W�F�N�g�Ȃ�ړ�������
                collidedRb.isKinematic = false;
                isRecentlyReleased = false;  // �t���O���Z�b�g
            }
        }
    }
}