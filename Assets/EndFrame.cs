using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFrame : StateMachineBehaviour
{
    public GameObject player;

    Player_Script player_Script;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player_Script = player.GetComponent<Player_Script>();
        player_Script.anim.SetBool("firstAttack",false);
    }
}
