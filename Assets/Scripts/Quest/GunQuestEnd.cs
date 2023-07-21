using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunQuestEnd : GunQuest
{

    protected override void Awake() {
        doorToOpen = GameObject.FindGameObjectWithTag("DoorForNextLevel");
        Texture2D sprite = Resources.Load<Texture2D>("Textures/gun");
        questUIInfo = new QuestUIInfo{
            questItemImage = sprite,
            questStatus = "0/1",
            questText = "Pick up Gun"
        };    
    }

    public override string GetQuestText(){
        return QuestText();
    }

    private string QuestText(){
        if(isQuestCompleted)
        {
            return "Good job! We will need your help, please come inside.";
        } 
        else
        {
            return "Hey! Did you bring that gun?";
        }
    }

}