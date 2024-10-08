using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visit : MonoBehaviour
{
    [SerializeField] private string[] targetNames;
    //[SerializeField] GameObject TimeBar;
    public Transform[] targets;

    private float speed = 3.0f;

    int seatnum = 0;

    int P = 0;

    GameObject A;
    emptyseat empty;
    // Start is called before the first frame update
    void Start()
    {
        targets = new Transform[targetNames.Length];

        for (int i = 0; i < targetNames.Length; i++)
        {
            GameObject targetObject = GameObject.Find(targetNames[i]);
            if (targetObject != null)
            {
                targets[i] = targetObject.transform;
            }
        }

        A = GameObject.Find("emptyseat");
        empty = A.GetComponent<emptyseat>();

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0;i < targets.Length; i++)
        {
            if (empty.seat[i] == true && P == 0)
            {
                seatnum = i;
                empty.seat[i] = false;
                P++;
            }
        }

        if (targets.Length > 0 && targets[seatnum] != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targets[seatnum].position, speed * Time.deltaTime);
        }
    }
}
