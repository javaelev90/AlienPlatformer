using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunQuest : GeneralQuest
{

    protected bool isQuestCompleted = false;
    public GameObject doorToOpen;
    protected QuestUIInfo questUIInfo;

    

    protected virtual void Awake()
    {
        doorToOpen = GameObject.FindGameObjectWithTag("DoorForGun");
        Texture2D sprite = Resources.Load<Texture2D>("Textures/gun");
        questUIInfo = new QuestUIInfo{
            questItemImage = sprite,
            questStatus = "0/1",
            questText = "Pick up Gun"
        }; 
    }

    void Update()
    {
        if(!isQuestCompleted)
        {
            IsQuestCompleted();
        }
    }

    public override bool IsQuestCompleted(){

        if (GetComponent<Inventory>().ContainsItem("MachineGun"))
        {
            isQuestCompleted = true;
            questUIInfo.questStatus = "1/1";
        }
        return isQuestCompleted;
    }

    public override void CompleteQuest()
    {
        doorToOpen.SetActive(false);
        isQuestActive = false;
    }

    public override string GetQuestText()
    {
        return QuestText();
    }

    public override QuestUIInfo GetQuestUIInfo()
    {
        return questUIInfo;
    }

    private string QuestText(){
        if(isQuestCompleted)
        {
            return "Good! You can fire the gun using [left-mouse-button]. You can reload with [R]. Please continue forward!";
        } 
        else
        {
            return "Eh! How did you get here?! Hmm, if you agree to bring that gun to big me, I will let you through. Pick up the gun to accept.";
        }
    }

}
