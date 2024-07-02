using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectToggle : MonoBehaviour
{
    public GameObject targetObject; // 表示/非表示にするオブジェクト
    private Vector3 originalScale; // オブジェクトの元のスケール
    private bool isRightClicking = false; // 右クリックが押されているかどうか
    private Coroutine scaleCoroutine;

    void Start()
    {
        // スタート時にオブジェクトを非表示にし、元のスケールを記憶する
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            originalScale = targetObject.transform.localScale;
        }
    }

    void Update()
    {
        bool wasRightClicking = isRightClicking;
        CheckRightClick();

        if (isRightClicking)
        {
            if (targetObject != null && !targetObject.activeSelf)
            {
                targetObject.SetActive(true);
                if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
                scaleCoroutine = StartCoroutine(ScaleObjectOverTime(Vector3.zero, originalScale, 0.1f));
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == targetObject)
                {
                    // オブジェクトにマウスが重なっている場合
                    ScaleObject(1.2f); // オブジェクトを1.2倍にする
                }
                else
                {
                    // オブジェクトからマウスが離れている場合
                    ResetScale(); // オブジェクトのスケールを元に戻す
                }
            }
            else
            {
                // ヒットしたオブジェクトがない場合も元に戻す
                ResetScale();
            }
        }
        else if (wasRightClicking && targetObject.activeSelf)
        {
            if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
            scaleCoroutine = StartCoroutine(ScaleObjectOverTime(targetObject.transform.localScale, Vector3.zero, 0.1f, () => targetObject.SetActive(false)));
        }
    }

    IEnumerator ScaleObjectOverTime(Vector3 fromScale, Vector3 toScale, float duration, System.Action onComplete = null)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            targetObject.transform.localScale = Vector3.Lerp(fromScale, toScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.localScale = toScale;
        onComplete?.Invoke();
    }

    void ScaleObject(float scaleMultiplier)
    {
        if (targetObject != null)
        {
            targetObject.transform.localScale = originalScale * scaleMultiplier; // スケールを変更する
        }
    }

    void ResetScale()
    {
        if (targetObject != null)
        {
            targetObject.transform.localScale = originalScale; // 元のスケールに戻す
        }
    }

    void CheckRightClick()
    {
        bool rightMouseButton = Input.GetMouseButton(1); // 右クリックの入力
        bool rightStickButton = false; // 右スティック押し込みの入力

        // XRデバイスの入力を取得
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, devices);

        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool stickClick))
            {
                rightStickButton = stickClick;
                break;
            }
        }

        isRightClicking = rightMouseButton || rightStickButton;
    }
}