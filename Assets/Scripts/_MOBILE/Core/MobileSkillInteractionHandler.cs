using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

public class MobileSkillInteractionHandler : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent <PlayerController>();
    }
    public void ProcessSkill(int _skillNum){
        Debug.Log("Clicked on skill button: "+_skillNum);
        playerController.SkillManager.PlaySkillAnimation(_skillNum);
    }
}
