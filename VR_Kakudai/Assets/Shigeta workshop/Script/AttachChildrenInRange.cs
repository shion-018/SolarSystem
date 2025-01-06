using UnityEngine;

public class AttachSushiChildrenInRange : MonoBehaviour
{
    public Transform parentObject; // 親にしたいオブジェクト
    public Vector3 boxSize = new Vector3(1f, 1f, 1f); // 基本の範囲のサイズ（ローカルスケールと乗算される）
    public Vector3 boxOffset = Vector3.zero; // 親オブジェクトからのオフセット

    public int[] DonburiDishes = new int[3] {999,999,999};
    int i = 0;

    void Update()
    {
        // ボタンが押された瞬間を検出
        //bool rightClick = OVRInput.GetDown(OVRInput.RawButton.A);
        bool keyPress = Input.GetKeyDown(KeyCode.A); // キーボードでの検出

        // いずれかのボタンが押された瞬間に処理を実行
        if (keyPress /*|| rightClick*/)
        {
            // ワールド座標で中心位置を計算
            Vector3 center = parentObject.position + boxOffset;

            // parentObjectのスケールを考慮した範囲サイズを計算
            Vector3 adjustedBoxSize = Vector3.Scale(boxSize, parentObject.localScale);

            // 範囲内のオブジェクトを検出
            Collider[] hitColliders = Physics.OverlapBox(center, adjustedBoxSize / 2);

           

            foreach (Collider collider in hitColliders)
            {
                // sushiタグを持つ場合のみ処理を実行
                if (collider.CompareTag("sushi"))
                {

                    DishesSetting ColDishesSetting = collider.gameObject.GetComponent<DishesSetting>();

                    // 親を設定
                    if (collider.transform.parent == null)
                    {
                        collider.transform.parent = parentObject;

                        if (DonburiDishes[i] == 999)
                        {
                            DonburiDishes[i] = ColDishesSetting.DishesNumber;
                            Debug.Log("DonbuiriDisihes = " + DonburiDishes[i]);
                            i++;
                        }

                    }

                    // Rigidbodyを無効化
                    Rigidbody rb = collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true; // 物理演算を無効化
                    }
                }
            }
        }
    }

    // シーンビューで範囲を可視化するためのデバッグ表示
    private void OnDrawGizmosSelected()
    {
        if (parentObject != null)
        {
            Gizmos.color = Color.green;
            Vector3 center = parentObject.position + boxOffset;

            // parentObjectのスケールを考慮した範囲サイズを計算
            Vector3 adjustedBoxSize = Vector3.Scale(boxSize, parentObject.localScale);
            Gizmos.DrawWireCube(center, adjustedBoxSize);
        }
    }
}
