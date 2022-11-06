using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

public class MobileSkillInteractionHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessSkill(int _skillNum){
        Debug.Log("Clicked on skill button: "+_skillNum);
        playerController.skillManager.PlaySkillAnimation(_skillNum);
    }
}
