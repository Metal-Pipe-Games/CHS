using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTool : StateMachineBehaviour
{
    public Tools tool = Tools.Gun;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var h = animator.gameObject.GetComponent<ToolHandler>();
        h.activeTool = tool;
        h.tools[h.activeTool].ResetTool();
    }
}
