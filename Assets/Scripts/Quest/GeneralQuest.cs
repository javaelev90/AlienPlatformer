using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralQuest : MonoBehaviour
{

    protected bool isQuestActive = true;
    public abstract bool IsQuestCompleted();
    public abstract void CompleteQuest();
    public abstract string GetQuestText();

    public bool IsQuestActive(){
        return isQuestActive;
    }

    public abstract QuestUIInfo GetQuestUIInfo();
    public void RemoveQuest(){
        //Destroy(this);
    }

}
