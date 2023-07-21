using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTip : MonoBehaviour
{

    [SerializeField] private GameObject UI_TipPanel;
    [SerializeField] private string triggerCollisionTag;
    private Quaternion initialRotation;

    private void Start()
    {
        UI_TipPanel.SetActive(false);
        initialRotation = transform.rotation;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(triggerCollisionTag))
        {
            UI_TipPanel.transform.rotation = initialRotation;
            UI_TipPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(triggerCollisionTag))
        {
            UI_TipPanel.SetActive(false);
        }
    }
}
