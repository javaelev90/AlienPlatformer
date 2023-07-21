using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_QuestPanel : MonoBehaviour
{
    [SerializeField] private List<UI_Quest> questUISlots;
    private List<QuestSlot> questSlots;
    private const int MAX_QUEST_COUNT = 2;
    [SerializeField] private GameObject player;

    [SerializeField] private Color completedQuestColor;
    
    private void Start() {
        questSlots = new List<QuestSlot>();
        foreach(UI_Quest questSlot in questUISlots){
            questSlot.gameObject.SetActive(false);
            questSlots.Add(new QuestSlot{
                questUI = questSlot,
                isUsed = false
            });
        }
        
    }

    private void Update() {
        GeneralQuest[] quests = player.GetComponents<GeneralQuest>();   
        
        foreach(GeneralQuest quest in quests){

            QuestSlot questSlot = GetQuestSlot(quest.GetInstanceID());

            // Clear out finished quests
            if(!quest.IsQuestActive() && questSlot != null){
                
                questSlot.questUI.ClearQuestInfo();
                questSlot.questUI.gameObject.SetActive(false);
                questSlot.Free();

            }

            // Find new quests and add it to quest UI
            if(quest.IsQuestActive() && !HasQuestSlot(quest.GetInstanceID())){
                
                questSlot = GetUnusedQuestSlot();
                
                if(questSlot != null){

                    questSlot.isUsed = true;
                    questSlot.usedByInstanceId = quest.GetInstanceID();
                    questSlot.questUI.DisplayQuestInfo(quest.GetQuestUIInfo());

                    questSlot.questUI.gameObject.SetActive(true);
                }
            }

            // Update existing quests
            if(quest.IsQuestActive() && HasQuestSlot(quest.GetInstanceID())){
                
                questSlot.questUI.DisplayQuestInfo(quest.GetQuestUIInfo());
            }
            

            if(quest.IsQuestActive() && quest.IsQuestCompleted()){
                questSlot.questUI.background.color = completedQuestColor;
            }
        
        }

    }

    private QuestSlot GetUnusedQuestSlot(){
        foreach(QuestSlot questSlot in questSlots){
            if(!questSlot.isUsed){
                return questSlot;
            }
        }
        return null;
    }

    private QuestSlot GetQuestSlot(int instanceId){
        foreach(QuestSlot questSlot in questSlots){
            if(questSlot.usedByInstanceId == instanceId){
                return questSlot;
            }
        }
        return null;
    }

    private bool HasQuestSlot(int instanceId){
        foreach(QuestSlot questSlot in questSlots){
            if(questSlot.usedByInstanceId == instanceId){
                return true;
            }
        }
        return false;
    }

    private class QuestSlot{
        public bool isUsed;
        public int usedByInstanceId;
        public UI_Quest questUI;

        public void Free(){
            isUsed = false;
            usedByInstanceId = -1;
        }
    }

}