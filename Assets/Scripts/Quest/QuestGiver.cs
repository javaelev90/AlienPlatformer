using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestGiver : MonoBehaviour
{

    public enum Quests {
        None,
        GunQuest,
        GunQuestEnd,
        SonarGunQuest,
        MonsterHuntQuest
    }
    public Quests firstQuest;
    public Quests secondQuest;
    private GeneralQuest firstGeneralQuest;
    private GeneralQuest secondGeneralQuest;
    [SerializeField] private GameObject questGiverText;
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private Button windowButton;
    private string questBeginnigText;
    private string questCompletionText;
    // private GameObject doorToOpenWhenQuestIsCompleted;
    [SerializeField] private bool shouldOffsetMainCamera = true;
    [SerializeField] private Vector2 mainCameraOffset;
    private Vector2 originalCameraLensShift;
    private IEnumerator tiltScreenCoroutine;
    private IEnumerator rollTextCoroutine;
    private List<string> questTextQueue;
    private int questTextQueueIndex = -1;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        questGiverText.SetActive(false);
        questTextQueue = new List<string>();
        originalCameraLensShift = Camera.main.lensShift;
    }

    private void Update() {
        if(windowButton.GetComponentInChildren<TMP_Text>() != null){
            if(questTextQueue.Count > 1){
                windowButton.GetComponentInChildren<TMP_Text>().text = "Next...";
            }
            if(questTextQueueIndex == questTextQueue.Count-1){
                windowButton.GetComponentInChildren<TMP_Text>().text = "Close";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            player.GetComponent<PlayerStats>().TogglePlayerLock();
            questTextQueue.Clear();
            //First quest
            if(GetQuestClassName(firstQuest) != null){
                
                firstGeneralQuest = collision.GetComponent(System.Type.GetType(GetQuestClassName(firstQuest))) as GeneralQuest;
                
                // If quest not picked up
                if(firstGeneralQuest == null)
                {
                    firstGeneralQuest = AddQuestComponentToGameObject(firstQuest, collision.gameObject);
                } 
                if(firstGeneralQuest.IsQuestActive()){
                    // If quest completed
                    if(firstGeneralQuest != null && firstGeneralQuest.IsQuestCompleted())
                    {       
                        firstGeneralQuest.CompleteQuest(); 
                    }
                    questTextQueue.Add(firstGeneralQuest.GetQuestText());
                }
            }

            //Second quest
            if(firstGeneralQuest.IsQuestCompleted()){
                if(GetQuestClassName(secondQuest) != null){
                    
                    secondGeneralQuest = collision.GetComponent(System.Type.GetType(GetQuestClassName(secondQuest))) as GeneralQuest;
                    
                    // If quest not picked up
                    if(secondGeneralQuest == null)
                    {
                        secondGeneralQuest = AddQuestComponentToGameObject(secondQuest, collision.gameObject);
                    } 
                    if(secondGeneralQuest.IsQuestActive()){
                        // If quest completed
                        if(secondGeneralQuest != null && secondGeneralQuest.IsQuestCompleted())
                        {       
                            secondGeneralQuest.CompleteQuest(); 
                        }
                        questTextQueue.Add(secondGeneralQuest.GetQuestText());
                    }
                }  
            }

            if (shouldOffsetMainCamera)
            {
                TriggerScreenTilt(mainCameraOffset);
            }
            questGiverText.SetActive(true);
            NextTextSegment();
        }
    }

    private GeneralQuest AddQuestComponentToGameObject(Quests quest, GameObject otherGameObject){
        if(quest == Quests.GunQuest){
            return otherGameObject.AddComponent<GunQuest>();
        } else if(quest == Quests.GunQuestEnd){
            return otherGameObject.AddComponent<GunQuestEnd>();
        } else if(quest == Quests.SonarGunQuest){
            return otherGameObject.AddComponent<SonarMonsterQuest>();
        } else if(quest == Quests.MonsterHuntQuest){
            return otherGameObject.AddComponent<MonsterHuntQuest>();
        } else {
            return null;
        }
    }

    public static string GetQuestClassName(Quests quest){
        if(quest == Quests.GunQuest){
            return typeof(GunQuest).Name;
        } else if(quest == Quests.GunQuestEnd){
            return typeof(GunQuestEnd).Name;
        } else if(quest == Quests.SonarGunQuest){
            return typeof(SonarMonsterQuest).Name;
        } else if(quest == Quests.MonsterHuntQuest){
            return typeof(MonsterHuntQuest).Name;
        } else {
            return null;
        }

    }

    public void NextTextSegment(){

        // There is no more text and window should close
        if(questTextQueueIndex == questTextQueue.Count-1)
        {
            CloseQuestWindow();
            return;
        } 

        questTextQueueIndex++;
        string text = questTextQueue[questTextQueueIndex];
        if(rollTextCoroutine != null){
            StopCoroutine(rollTextCoroutine);
        }
        rollTextCoroutine = RollText(text);
        StartCoroutine(rollTextCoroutine);

    }

    private IEnumerator RollText(string text){
        string originalText = text;
        for(int i = 0; i < originalText.Length; i++){
            textComponent.text = originalText.Substring(0,i); 
            yield return new WaitForSeconds(0.015f);
        } 
    }

    private IEnumerator tiltScreen(Vector2 target)
    {
        while (Vector2.Distance(Camera.main.lensShift, target) >= 0.005f)
        {
            Camera.main.lensShift = Vector2.Lerp(Camera.main.lensShift, target, Time.deltaTime + 0.05f);
            yield return null;
        }
    }

    private void CloseQuestWindow(){
        player.GetComponent<PlayerStats>().TogglePlayerLock();
        questGiverText.SetActive(false);
        questTextQueueIndex = -1;
        if (shouldOffsetMainCamera)
        {
            TriggerScreenTilt(originalCameraLensShift);
        }
    }

    private void TriggerScreenTilt(Vector2 target)
    {
        if (tiltScreenCoroutine != null)
        {
            StopCoroutine(tiltScreenCoroutine);
        }
        tiltScreenCoroutine = tiltScreen(target);
        StartCoroutine(tiltScreenCoroutine);
    }


}
