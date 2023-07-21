using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RawImage image;
    [SerializeField] private int levelToLoad;
    [SerializeField] private RectTransform controlsMenu;
    [SerializeField] private Button startButton, quitButton, controlsButton, closeControlsButton;

    private float timer = 0f;
    private float colorChangeCountdown = 0.5f;
    void Start()
    {
        levelToLoad = 1;
        startButton.onClick.AddListener(() => StartGame());
        quitButton.onClick.AddListener(() => Application.Quit());
        controlsButton.onClick.AddListener(() => ToggleControlsMenu());
        closeControlsButton.onClick.AddListener(() => ToggleControlsMenu());
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > colorChangeCountdown){
            startButton.image.color = GetColor();
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    private void StartGame(){
        SceneManager.LoadScene(levelToLoad);
    }

    private void ToggleControlsMenu(){
        controlsMenu.gameObject.SetActive(!controlsMenu.gameObject.activeSelf);
    }

    private Color GetColor(){
        //HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * Random.Range(0f, 1f), 1), 1, 1));//Random.ColorHSV();
        if(startButton.image.color == Color.black){
            return Color.white;
        } else {
            return Color.black;
        }
    }
}
