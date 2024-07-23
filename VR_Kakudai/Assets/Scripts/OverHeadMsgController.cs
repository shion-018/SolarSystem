using UnityEngine;
using UnityEngine.UI;

public class OverHeadMsgController : MonoBehaviour
{
    [SerializeField]
    Text overHeadMsg;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            overHeadMsg.text = "Hello";
        }
    }
}