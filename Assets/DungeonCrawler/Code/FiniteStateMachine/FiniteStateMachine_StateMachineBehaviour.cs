using MrSanmi.DungeonCrawler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FiniteStateMachine_StateMachineBehaviour : StateMachineBehaviour
{
    public States state;

    // This method checks the first moment where you enter a state.
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<FiniteStateMachine>().SetState(state);
    }
}
