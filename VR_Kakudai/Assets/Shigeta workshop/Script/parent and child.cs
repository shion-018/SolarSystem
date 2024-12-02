using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parentandchild : MonoBehaviour
{

    private GameObject targetSushi = null;

    void Update()
    {
        bool rightClick = OVRInput.GetDown(OVRInput.RawButton.A);
        Transform currentParent = this.transform.parent;

        if (Input.GetKey(KeyCode.A) || rightClick)
            if (targetSushi != null)
            {
                targetSushi.transform.SetParent(this.transform);
                targetSushi = null; // àÍìxê›íËÇµÇΩÇÁÉNÉäÉA

            }

        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("sushi"))
        {
            targetSushi = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetSushi)
        {
            targetSushi = null;
        }
    }
}
