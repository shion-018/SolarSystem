using UnityEngine;

public class DayLight : MonoBehaviour
{
    public Light sunLight;             // 太陽の光源オブジェクト
    public float dayDuration = 300f;   // 1日の長さ（秒で指定、ここでは300秒=5分）

    private float rotationSpeed;
    private float currentRotation = 0f; // 現在の回転角度

    void Start()
    {
        // 回転速度を180度で計算（5分で180度）
        rotationSpeed = 180f / dayDuration;
        ResetSunPosition();
    }

    void Update()
    {
        // Rキーが押されたら太陽の位置をリセット
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetSunPosition();
        }

        // 現在の回転角度が180度未満の場合のみ回転を進める
        if (currentRotation < 181f)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            sunLight.transform.Rotate(Vector3.right, rotationStep);
            currentRotation += rotationStep;
        }
    }

    // 太陽の位置を初期状態にリセットするメソッド
    void ResetSunPosition()
    {
        sunLight.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        currentRotation = 0f;
    }
}