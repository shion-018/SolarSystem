using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visit : MonoBehaviour
{
    [SerializeField] private string[] targetNames;
    
    public Transform[] targets;

    [SerializeField] private string[] RoadName0;

    [SerializeField] private string[] RoadName1;

    [SerializeField] private string[] RoadName2;

    int RoadNum = 0;

    bool TriggerSignal = false;

    public Transform[] Road;

    private float speed = 3.0f;

    int seatnum = 0;

    int P = 0;

    bool HeadtoGirl = false;

    public GameObject cube;
    public BoxCollider cube_boxCol;

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

        cube = GameObject.Find("Cube");
        cube_boxCol = cube.GetComponent<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (P == 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (empty.seat[i] == true && P == 0)
                {
                    seatnum = i;
                    empty.seat[i] = false;
                    P++;
                    InputRoad(seatnum);
                }
            }
        }

        if(HeadtoGirl == false && RoadNum < Road.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, Road[RoadNum].position, speed * Time.deltaTime);

            if(TriggerSignal == true)
            {
                
                RoadNum++;
                Debug.Log(RoadNum);
                TriggerSignal = false;
            }

            if(RoadNum > Road.Length-1)
            {
                HeadtoGirl = true;
            }
        }


        if (targets.Length > 0 && targets[seatnum] != null && HeadtoGirl == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targets[seatnum].position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Road"))
        {
            TriggerSignal = true;
        }

        if (other.gameObject.CompareTag("Finish")){
            cube_boxCol.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.CompareTag("sushi"))
        {
            empty.seat[seatnum] = true;
            Destroy(this.gameObject);
        }

    }

    void InputRoad(int seatnum)
    {
        switch (seatnum)
        {
            case 0:
                Road = new Transform[RoadName0.Length];
                for (int i = 0; i < RoadName0.Length; i++)
                {
                    GameObject targetObject = GameObject.Find(RoadName0[i]);
                    if (targetObject != null)
                    {
                        Road[i] = targetObject.transform;
                    }
                }
                break;
            case 1:
                Road = new Transform[RoadName1.Length];
                for (int i = 0; i < RoadName1.Length; i++)
                {
                    GameObject targetObject = GameObject.Find(RoadName1[i]);
                    if (targetObject != null)
                    {
                        Road[i] = targetObject.transform;
                    }
                }
                break;
            case 2:
                Road = new Transform[RoadName2.Length];
                for (int i = 0; i < RoadName2.Length; i++)
                {
                    GameObject targetObject = GameObject.Find(RoadName2[i]);
                    if (targetObject != null)
                    {
                        Road[i] = targetObject.transform;
                    }
                }
                break;

        }
    }
}
