using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moob : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    [SerializeField] GameObject RootObject;

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;
        this.gameObject.transform.parent = null;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "donburi")
        {
            this.gameObject.transform.parent = RootObject.gameObject.transform;
        }
    }
}
