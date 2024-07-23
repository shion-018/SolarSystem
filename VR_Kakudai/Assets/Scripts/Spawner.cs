using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject CubePrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Instantiate(CubePrefab);
        }
    }
}