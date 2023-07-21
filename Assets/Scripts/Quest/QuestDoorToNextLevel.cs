using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestDoorToNextLevel : MonoBehaviour
{

    [SerializeField] private int levelToLoad;
    public QuestGiver.Quests questToUnlockDoor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if ((collision.GetComponent(QuestGiver.GetQuestClassName(questToUnlockDoor)) as GeneralQuest).IsQuestCompleted())
            {
                SceneManager.LoadScene(levelToLoad);
            }
        }
    }
}
