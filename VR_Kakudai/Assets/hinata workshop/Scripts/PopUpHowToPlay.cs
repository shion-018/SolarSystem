using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
public class PopUpHowToPlay : MonoBehaviour
{
    [SerializeField] private Grabbable _grabbable; // Grabbableオブジェクト
    [SerializeField] private GameObject _panel; // 表示・非表示にするパネル
    [SerializeField] private Vector3 _offset = new Vector3(0, 0.2f, 0); // パネルの位置オフセット

    private Camera _mainCamera;

    private void Start()
    {
        // メインカメラを取得
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        // オブジェクトがつかまれたときのイベントリスナーを設定
        _grabbable.WhenPointerEventRaised += OnPointerEvent;
    }

    private void OnDisable()
    {
        // イベントリスナーを解除
        _grabbable.WhenPointerEventRaised -= OnPointerEvent;
    }

    private void OnPointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
        {
            // オブジェクトの上にパネルを配置
            PositionPanelAboveObject();
            _panel.SetActive(true); // パネルを表示
        }
        else if (evt.Type == PointerEventType.Unselect)
        {
            _panel.SetActive(false); // パネルを非表示
        }
    }

    private void PositionPanelAboveObject()
    {
        // グラブされたオブジェクトのTransformを取得
        Transform grabbedObjectTransform = _grabbable.transform;

        // パネルの位置をオブジェクトのやや上に設定
        _panel.transform.position = grabbedObjectTransform.position + _offset;

        // パネルをカメラの方に向ける
        _panel.transform.LookAt(_mainCamera.transform);

        // パネルが反対方向を向く場合は、180度回転させる
        _panel.transform.Rotate(0, 180, 0);
    }
}
