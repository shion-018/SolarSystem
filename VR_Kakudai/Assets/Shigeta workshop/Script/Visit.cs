using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visit : MonoBehaviour
{
    [SerializeField] private string[] targetNames;
    [SerializeField] GameObject TimeBar;
    private Transform[] targets;

    private float speed = 3.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (targets.Length > 0 && targets[0] != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targets[0].position, speed * Time.deltaTime);
        }
    }
}
