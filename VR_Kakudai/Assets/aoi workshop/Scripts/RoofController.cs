using UnityEngine;

public class RoofController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("飛んでいく方向と力の大きさ")]
    public Vector3 launchDirection = new Vector3(1f, 2f, 0f);

    [Tooltip("回転の速さ")]
    public float rotationSpeed = 360f;

    [Tooltip("飛んでいく力の大きさ")]
    public float launchForce = 5f;

    private Rigidbody rb;
    private bool isLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 初期設定：重力の影響を受けず、物理演算は有効
        rb.useGravity = false;
        rb.isKinematic = false;

        // 回転しないように固定
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトの情報をログ出力
        Debug.Log($"衝突検出: オブジェクト名={collision.gameObject.name}, タグ={collision.gameObject.tag}");

        // まだ飛んでいない状態で、寿司タグのついたオブジェクトと衝突した場合
        if (!isLaunched && collision.gameObject.CompareTag("sushi"))
        {
            Debug.Log("寿司と衝突しました。屋根を発射します。");
            Launch();
        }
    }

    void Launch()
    {
        isLaunched = true;

        // 物理挙動の制約を解除
        rb.constraints = RigidbodyConstraints.None;

        // 重力を有効化
        rb.useGravity = true;

        // 力を加えて飛ばす
        rb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);

        // 回転を加える
        rb.AddTorque(transform.right * rotationSpeed);

        Debug.Log("屋根を発射しました: 力と回転を加えました。");
    }
}