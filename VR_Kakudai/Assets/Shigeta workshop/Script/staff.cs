using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] private Transform[] target;
    private GameObject child;
    private float speed = 3.0f;

    private enum Dishes
    {
        Nohave,
        Havedishes
    }

    Dishes dishes = Dishes.Nohave;

    // This method is called externally when the child is spawned
    public void SetChild(GameObject childObject)
    {
        child = childObject;
    }

    // Update is called once per frame
    void Update()
    {
        switch (dishes)
        {
            case Dishes.Nohave:
                transform.position = Vector3.MoveTowards(transform.position, target[0].position, speed * Time.deltaTime);
                if (this.transform.position == target[0].transform.position && child != null)
                {
                    //ワールド座標を保持
                    Vector3 childWorldPosition = child.transform.position;

                    //親子関係を設定
                    child.transform.SetParent(this.transform);

                    //ワールド座標を再設定
                    child.transform.position = childWorldPosition;

                    transform.rotation = Quaternion.AngleAxis(90.0f, new Vector3(0, 1, 0));

                    if (gameObject.transform.localEulerAngles.y <= 90)
                    {
                        dishes = Dishes.Havedishes;
                    }
                }
                break;

            case Dishes.Havedishes:
                transform.position = Vector3.MoveTowards(transform.position, target[1].position, speed * Time.deltaTime);
                break;
        }
    }
}
