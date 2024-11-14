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
    GameObject B;
    CustomerCounter CustmerCounter;
    GameObject C;
    Appeardishes appeardishes;

    // PlayerのTransformを取得するための変数を追加
    private Transform playerTransform;

    // アニメーションの管理用
    private Animator animator;

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

        B = GameObject.Find("CustonerCount");
        CustmerCounter = B.GetComponent<CustomerCounter>();

        C = GameObject.Find("DishesManager");
        appeardishes = C.GetComponent<Appeardishes>();

        cube_boxCol = this.GetComponent<BoxCollider>();

        // Playerオブジェクトを取得してTransformを保持
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        // Animatorコンポーネントの取得
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isMoving = false;

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

        if (HeadtoGirl == false && RoadNum < Road.Length)
        {
            Vector3 direction = (Road[RoadNum].position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, Road[RoadNum].position, speed * Time.deltaTime);

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            if (TriggerSignal)
            {
                RoadNum++;
                Debug.Log(RoadNum);
                TriggerSignal = false;
            }

            if (RoadNum > Road.Length - 1)
            {
                HeadtoGirl = true;
            }

            isMoving = true;
        }

        if (targets.Length > 0 && targets[seatnum] != null && HeadtoGirl == true)
        {
            Vector3 directionToTarget = (targets[seatnum].position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targets[seatnum].position, speed * Time.deltaTime);

            if (directionToTarget != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToTarget);
            }

            // Counterポジションに到達したらPlayerの方向を向く
            if (Vector3.Distance(transform.position, targets[seatnum].position) < 0.1f && playerTransform != null)
            {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(directionToPlayer);

                // アニメーションを待機状態に切り替える
                isMoving = false;
            }
            else
            {
                isMoving = true;
            }
        }

        // アニメーションの切り替え
        if (animator != null)
        {
            animator.SetBool("walking", isMoving);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Road"))
        {
            TriggerSignal = true;
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            InputDishes();
            cube_boxCol.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("sushi"))
        {
            empty.seat[seatnum] = true;
            CustmerCounter.counter--;
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

    void InputDishes()
    {
        switch (seatnum)
        {
            case 0:
                appeardishes.dishesnum = 0;
                appeardishes.spawndishes[0] = true;
                break;
            case 1:
                appeardishes.dishesnum = 1;
                appeardishes.spawndishes[1] = true;
                break;
            case 2:
                appeardishes.dishesnum = 2;
                appeardishes.spawndishes[2] = true;
                break;
        }
    }
}
