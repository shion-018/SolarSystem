using UnityEngine;

public class OverHeadMsgCreater : MonoBehaviour
{

    public RectTransform canvasRect;

    [SerializeField]
    OverHeadMsg overHeadMsgPrefab;

    OverHeadMsg overHeadMsg;

    void Start()
    {
        overHeadMsg = Instantiate(overHeadMsgPrefab, canvasRect);
        overHeadMsg.targetTran = transform;
    }

    void OnEnable()
    {
        if (overHeadMsg == null) return;

        overHeadMsg.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            overHeadMsg.ShowMsg("Hello");
        }
    }

    void OnDisable()
    {
        if (overHeadMsg == null) return;

        overHeadMsg.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (overHeadMsg == null) return;

        Destroy(overHeadMsg.gameObject);
    }
}