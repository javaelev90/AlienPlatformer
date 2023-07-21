using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarMonsterQuest : GeneralQuest
{

    protected bool isQuestCompleted = false;
    public GameObject doorToOpen;
    protected QuestUIInfo questUIInfo;
    private void Awake()
    {
        doorToOpen = GameObject.FindGameObjectWithTag("DoorForGun");
        Texture2D sprite = Resources.Load<Texture2D>("Textures/sonargun");
        questUIInfo = new QuestUIInfo{
            questItemImage = sprite,
            questStatus = "0/1",
            questText = "Pick up SonarGun"
        };
    }
    public override bool IsQuestCompleted(){
        if (GetComponent<Inventory>().ContainsItem("SonarGun"))
        {
            isQuestCompleted = true;
            questUIInfo.questStatus = "1/1";
        }
        return isQuestCompleted;
    }
    public override void CompleteQuest(){
        doorToOpen.SetActive(false);
        isQuestActive = false;
    }
    public override string GetQuestText(){
        return QuestText();
    }

    public override QuestUIInfo GetQuestUIInfo()
    {
        return questUIInfo;
    }

    private string QuestText(){
        if(isQuestCompleted)
        {
            return "Good! You can fire the gun using [left-mouse-button]. The sonar gun will make a map showing terrain, enemies and hazards.";
        } 
        else
        {
            return "Hi again! The light on this level does not work very well. Please pick up the sonar gun, it will help you in the dark.";
        }
    }
}
