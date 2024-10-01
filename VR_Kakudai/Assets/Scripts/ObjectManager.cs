using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public float throwForceMultiplier = 0.1f;

    private Camera cam;
    private GameObject selectedObject;
    private Rigidbody selectedObjectRb;
    private Vector3 lastMousePosition;
    private bool isGrabbing = false;

    // �Ō�ɐ������ꂽ�I�u�W�F�N�g��ǐ�
    private GameObject lastSpawnedObject;

    // VR�R���g���[���̏�Ԃ�ێ�
    private InputDevice rightHandController;

    void Start()
    {
        cam = Camera.main;

        // �E���VR�R���g���[���f�o�C�X���擾
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

        if (devices.Count > 0)
        {
            rightHandController = devices[0];
            Debug.Log("Right hand controller found: " + rightHandController.name);
        }
        else
        {
            Debug.LogWarning("No right hand controller found!");
        }
    }

    void Update()
    {
        HandleRightClickSpawnOrDelete(); // �E�N���b�N�ŃI�u�W�F�N�g�����܂��͍폜
        HandleGrab();            // ���N���b�N�ŃI�u�W�F�N�g��͂�
        HandleRelease();         // ���N���b�N�𗣂��ƃI�u�W�F�N�g�����
    }

    // �E�N���b�N�ŐV�����I�u�W�F�N�g�𐶐����邩�A�����̃I�u�W�F�N�g���폜
    void HandleRightClickSpawnOrDelete()
    {
        bool rightClick = Input.GetMouseButtonDown(1) || GetRightControllerButtonPress(CommonUsages.primaryButton);

        if (rightClick)
        {
            Debug.Log("Right click (B button) detected!");

            if (isGrabbing)
            {
                return; // �͂�ł���Ԃ̓I�u�W�F�N�g�����ł����Ȃ�
            }

            if (lastSpawnedObject == null || lastSpawnedObject.GetComponent<Rigidbody>().useGravity)
            {
                SpawnObject(); // �V�����I�u�W�F�N�g�𐶐�
            }
            else
            {
                Destroy(lastSpawnedObject); // �����̃I�u�W�F�N�g���폜
                lastSpawnedObject = null;
            }
        }
    }

    // �V�����I�u�W�F�N�g�𐶐�����
    void SpawnObject()
    {
        GameObject newObject = Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody newObjectRb = newObject.GetComponent<Rigidbody>();
        newObjectRb.useGravity = false; // �������ɂ͏d�͂𖳌���
        lastSpawnedObject = newObject; // �Ō�ɐ������ꂽ�I�u�W�F�N�g���X�V
    }

    // ���N���b�N�ŃI�u�W�F�N�g��͂ޏ���
    void HandleGrab()
    {
        bool leftClick = Input.GetMouseButtonDown(0) || GetRightControllerButtonPress(CommonUsages.triggerButton);

        if (leftClick)
        {
            Debug.Log("Left click (RT button) detected!");

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Grabbable"))
            {
                selectedObject = hit.collider.gameObject;
                selectedObjectRb = selectedObject.GetComponent<Rigidbody>();
                selectedObjectRb.isKinematic = true; // �����������~
                selectedObjectRb.useGravity = false; // �͂�ł���Ԃ͏d�͂𖳌���
                lastMousePosition = Input.mousePosition;
                isGrabbing = true;
            }
        }

        if (isGrabbing && (Input.GetMouseButton(0) || GetRightControllerButtonPress(CommonUsages.triggerButton)))
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            Vector3 moveDirection = new Vector3(mouseDelta.x, mouseDelta.y, 0);
            selectedObject.transform.position += moveDirection * Time.deltaTime;
            lastMousePosition = Input.mousePosition;
        }
    }

    // ���N���b�N�𗣂��ƃI�u�W�F�N�g�������
    void HandleRelease()
    {
        bool leftClickRelease = Input.GetMouseButtonUp(0) || GetRightControllerButtonRelease(CommonUsages.triggerButton);

        if (isGrabbing && leftClickRelease)
        {
            Debug.Log("Left click release (RT button) detected!");

            selectedObjectRb.isKinematic = false; // �����������ĊJ
            selectedObjectRb.useGravity = true; // �d�͂��ėL����
            Vector3 mouseVelocity = (Input.mousePosition - lastMousePosition) * throwForceMultiplier; // ������t����
            selectedObjectRb.velocity = new Vector3(mouseVelocity.x, mouseVelocity.y, 5.0f); // �O���ɔ�΂�
            isGrabbing = false;
        }
    }

    // �R���g���[���̃{�^���������ꂽ�����`�F�b�N
    bool GetRightControllerButtonPress(InputFeatureUsage<bool> button)
    {
        if (rightHandController.isValid)
        {
            rightHandController.TryGetFeatureValue(button, out bool buttonPressed);
            return buttonPressed;
        }
        return false;
    }

    // �R���g���[���̃{�^���������ꂽ�����`�F�b�N
    bool GetRightControllerButtonRelease(InputFeatureUsage<bool> button)
    {
        if (rightHandController.isValid)
        {
            rightHandController.TryGetFeatureValue(button, out bool buttonReleased);
            return !buttonReleased;
        }
        return false;
    }
}