using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab; // 出現させるPrefab
    public Transform spawnPoint; // 出現位置

    private GameObject currentObject; // 現在の出現中のオブジェクト
    private bool isGrabbed = false; // オブジェクトが掴まれているか

    void Update()
    {
        // Bボタンを押したとき
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            // 掴まれていない場合
            if (!isGrabbed)
            {
                // 現在のオブジェクトが存在する場合は削除
                if (currentObject != null)
                {
                    Destroy(currentObject);
                    currentObject = null;
                }
                // 現在のオブジェクトがない場合は新しく出現
                else
                {
                    currentObject = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                    // Grabbableコンポーネントのイベントを監視
                    var grabbable = currentObject.GetComponent<OVRGrabbable>();
                    if (grabbable != null)
                    {
                        grabbable.OnGrabBegin += OnGrabBegin;
                        grabbable.OnGrabEnd += OnGrabEnd;
                    }
                }
            }
        }
    }

    // 掴み開始時の処理
    private void OnGrabBegin()
    {
        isGrabbed = true;
    }

    // 掴み終了時の処理
    private void OnGrabEnd()
    {
        isGrabbed = false;
    }

    private void OnDestroy()
    {
        // イベントを解除
        if (currentObject != null)
        {
            var grabbable = currentObject.GetComponent<OVRGrabbable>();
            if (grabbable != null)
            {
                grabbable.OnGrabBegin -= OnGrabBegin;
                grabbable.OnGrabEnd -= OnGrabEnd;
            }
        }
    }
}