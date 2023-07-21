using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthPanel : MonoBehaviour
{
    [SerializeField ] private PlayerStats playerStats;
    private Slider slider;
    int maxPlayerHealthPoints;
    // Start is called before the first frame update
    void Start()
    {
        maxPlayerHealthPoints = playerStats.playerInitialHealth;
        slider = gameObject.GetComponent<Slider>();
        slider.wholeNumbers = true;
        slider.maxValue = maxPlayerHealthPoints;
        slider.minValue = 0;
        slider.value = playerStats.playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = playerStats.playerHealth;
    }
}
