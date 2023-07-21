using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHuntQuest : GeneralQuest, IStateful
{

    
    protected bool isQuestCompleted = false;
    public GameObject doorToOpen;
    protected QuestUIInfo questUIInfo;
    private QuestState questState;
    private int monstersToKill = 5;
    private int monstersKilled = 0;
    private int prevMonstersKilled = 0;
    // Start is called before the first frame update
    void Start()
    {
        doorToOpen = GameObject.FindGameObjectWithTag("DoorForNextLevel");
        Texture2D sprite = Resources.Load<Texture2D>("Textures/monster-head");
        questUIInfo = new QuestUIInfo{
            questItemImage = sprite,
            questStatus = "0/5",
            questText = "Kill monster"
        }; 
        SaveState();
        GameMaster.Instance.AddPersistentStatefulObject(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(monstersKilled != prevMonstersKilled){
            questUIInfo.questStatus = monstersKilled + "/" + monstersToKill;
            prevMonstersKilled = monstersKilled;
        }
        IsQuestCompleted();
    }

    public override bool IsQuestCompleted(){

        if (monstersKilled == monstersToKill)
        {
            isQuestCompleted = true;
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


    public void KilledMonster(){
        if(monstersKilled < monstersToKill){
           monstersKilled++; 
        }
    }

    public void SaveState(){
        questState = new QuestState(){
            monstersKilled = monstersKilled
        };
    }
    public void LoadSavedState(){
        monstersKilled = questState.monstersKilled;
    }

    private string QuestText(){
        if(isQuestCompleted)
        {
            return "Good job! Lets continue further.";
        } 
        else
        {
            return "Now.. Alot of monster have moved in here and I need you to kill some. "
            + "You need to kill " + monstersToKill + " monsters."; 
        }
    }

    private class QuestState
    {
        public int monstersKilled;
    }
}
