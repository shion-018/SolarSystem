using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GrabController : MonoBehaviour
{
    public OVRInput.Controller oculusController = OVRInput.Controller.RTouch; // Oculusコントローラー
    public float grabRadius = 0.2f; // 掴む範囲
    public LayerMask grabbableLayer; // 掴めるオブジェクトのレイヤー
    public float grabHoldDuration = 0.5f; // 左クリック長押しの時間

    private GameObject grabbedObject; // 掴んだオブジェクト
    private float leftClickHoldTime = 0f; // 左クリック長押しのタイマー

    void Update()
    {
        // Oculusコントローラーの入力をチェック
        bool isOculusGrabbing = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, oculusController);

        // マウスの左クリック入力をチェック
        if (Input.GetMouseButton(0)) // 左クリックが押され続けている場合
        {
            leftClickHoldTime += Time.deltaTime;
        }
        else
        {
            leftClickHoldTime = 0f; // 離されたらリセット
        }

        bool isMouseGrabbing = leftClickHoldTime >= grabHoldDuration;

        // 掴むアクション
        if (isOculusGrabbing || isMouseGrabbing)
        {
            TryGrab();
        }

        // 掴んだオブジェクトを離す
        if ((OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, oculusController) || Input.GetMouseButtonUp(0)) && grabbedObject != null)
        {
            ReleaseGrab();
        }
    }

    void TryGrab()
    {
        if (grabbedObject != null) return; // 既に掴んでいる場合は処理しない

        // プレイヤーの周囲を探索
        Collider[] hits = Physics.OverlapSphere(transform.position, grabRadius, grabbableLayer);

        if (hits.Length > 0)
        {
            // 最初にヒットしたオブジェクトを掴む
            grabbedObject = hits[0].gameObject;
            grabbedObject.transform.SetParent(transform);
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true; // 物理挙動を無効化
        }
    }

    void ReleaseGrab()
    {
        // 掴んだオブジェクトを解放
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false; // 物理挙動を有効化
        grabbedObject.transform.SetParent(null);
        grabbedObject = null;
    }

    private void OnDrawGizmosSelected()
    {
        // 掴む範囲を視覚化
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}
