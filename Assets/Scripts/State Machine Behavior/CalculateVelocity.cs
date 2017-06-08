using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateVelocity : StateMachineBehaviour {

    public string valueToApplyTo;
    public string boolName;

    Vector3 oldPosition;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(valueToApplyTo, (oldPosition - animator.transform.parent.transform.position).sqrMagnitude);

        if ((oldPosition - animator.transform.parent.transform.position).sqrMagnitude >= Mathf.Epsilon)
            animator.SetBool(boolName, true);
        else
            animator.SetBool(boolName, false);

        oldPosition = animator.gameObject.transform.position;
    }
}
