using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staff : MonoBehaviour
{

    [SerializeField] private Transform[] target;
    private float speed = 3.0f;

    private enum Dishes
    {
        Nohave,
        havedishes

    }

    Dishes dishes = Dishes.Nohave;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        switch (dishes) {
            case Dishes.Nohave:
            transform.position = Vector3.MoveTowards(transform.position, target[0].position, speed * Time.deltaTime);
                if (this.transform.position == target[0].transform.position)
                {
                    dishes = Dishes.havedishes;
                }
                break;

                case Dishes.havedishes:
                transform.position = Vector3.MoveTowards(transform.position, target[1].position, speed * Time.deltaTime);
                break;

        }

    }
}
