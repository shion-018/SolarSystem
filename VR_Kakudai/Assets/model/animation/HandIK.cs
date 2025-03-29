using UnityEngine;

public class HandIK : MonoBehaviour
{
    public Animator animator;

    [Range(0, 1)]
    public float weight = 1;
    // Start is called before the first frame update
    void OnAnimatorIK()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, weight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, weight);
    }

   
}
