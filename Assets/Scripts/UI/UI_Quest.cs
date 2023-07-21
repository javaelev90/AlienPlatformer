using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Quest : MonoBehaviour
{
    // Start is called before the first frame update{
    public TMP_Text questText;
    public TMP_Text questStatus;
    public RawImage questItemImage;
    public Image background;
    private Color defaultBackgroundColor;

    private void Start() {
        defaultBackgroundColor = GetComponent<Image>().color;
    }

    public void DisplayQuestInfo(QuestUIInfo questUIInfo){
        questText.text = questUIInfo.questText;
        questStatus.text = questUIInfo.questStatus;
        questItemImage.texture = questUIInfo.questItemImage;
    }

    public void ClearQuestInfo(){
        questText.text = null;
        questStatus.text = null;
        questItemImage.texture = null;
        background.color = defaultBackgroundColor;
    }

}
