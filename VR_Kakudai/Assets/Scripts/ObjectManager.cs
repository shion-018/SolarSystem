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

    // 最後に生成されたオブジェクトを追跡
    private GameObject lastSpawnedObject;

    // VRコントローラの状態を保持
    private InputDevice rightHandController;

    void Start()
    {
        cam = Camera.main;

        // 右手のVRコントローラデバイスを取得
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
        HandleRightClickSpawnOrDelete(); // 右クリックでオブジェクト生成または削除
        HandleGrab();            // 左クリックでオブジェクトを掴む
        HandleRelease();         // 左クリックを離すとオブジェクトを放つ
    }

    // 右クリックで新しいオブジェクトを生成するか、既存のオブジェクトを削除
    void HandleRightClickSpawnOrDelete()
    {
        bool rightClick = Input.GetMouseButtonDown(1) || GetRightControllerButtonPress(CommonUsages.primaryButton);

        if (rightClick)
        {
            Debug.Log("Right click (B button) detected!");

            if (isGrabbing)
            {
                return; // 掴んでいる間はオブジェクトを消滅させない
            }

            if (lastSpawnedObject == null || lastSpawnedObject.GetComponent<Rigidbody>().useGravity)
            {
                SpawnObject(); // 新しいオブジェクトを生成
            }
            else
            {
                Destroy(lastSpawnedObject); // 既存のオブジェクトを削除
                lastSpawnedObject = null;
            }
        }
    }

    // 新しいオブジェクトを生成する
    void SpawnObject()
    {
        GameObject newObject = Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody newObjectRb = newObject.GetComponent<Rigidbody>();
        newObjectRb.useGravity = false; // 生成時には重力を無効に
        lastSpawnedObject = newObject; // 最後に生成されたオブジェクトを更新
    }

    // 左クリックでオブジェクトを掴む処理
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
                selectedObjectRb.isKinematic = true; // 物理挙動を停止
                selectedObjectRb.useGravity = false; // 掴んでいる間は重力を無効に
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

    // 左クリックを離すとオブジェクトを放つ処理
    void HandleRelease()
    {
        bool leftClickRelease = Input.GetMouseButtonUp(0) || GetRightControllerButtonRelease(CommonUsages.triggerButton);

        if (isGrabbing && leftClickRelease)
        {
            Debug.Log("Left click release (RT button) detected!");

            selectedObjectRb.isKinematic = false; // 物理挙動を再開
            selectedObjectRb.useGravity = true; // 重力を再有効化
            Vector3 mouseVelocity = (Input.mousePosition - lastMousePosition) * throwForceMultiplier; // 勢いを付ける
            selectedObjectRb.velocity = new Vector3(mouseVelocity.x, mouseVelocity.y, 5.0f); // 前方に飛ばす
            isGrabbing = false;
        }
    }

    // コントローラのボタンが押されたかをチェック
    bool GetRightControllerButtonPress(InputFeatureUsage<bool> button)
    {
        if (rightHandController.isValid)
        {
            rightHandController.TryGetFeatureValue(button, out bool buttonPressed);
            return buttonPressed;
        }
        return false;
    }

    // コントローラのボタンが離されたかをチェック
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