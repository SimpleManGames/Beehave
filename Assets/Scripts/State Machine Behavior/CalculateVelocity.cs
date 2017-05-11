using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateVelocity : StateMachineBehaviour {

    public string valueToApplyTo;

    Vector3 oldPosition;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(valueToApplyTo, (oldPosition - animator.transform.parent.transform.position).sqrMagnitude);
        oldPosition = animator.gameObject.transform.position;
    }
}
