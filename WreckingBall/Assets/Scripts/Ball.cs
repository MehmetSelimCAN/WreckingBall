using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

    public static Ball Instance { private set; get; }

    public static int combo;
    private float comboTimer;
    private float comboTimerMax = 3f;
    private Transform comboUI;
    private Text comboTextUI;
    private Image comboTimerImage1;
    private Image comboTimerImage2;

    private Transform livesUI;
    private Text scoreTextUI;
    public static int score;


    private void Awake() {
        Instance = this;

        comboUI = GameObject.Find("Combo").transform;
        comboTextUI = GameObject.Find("Combo").transform.Find("text").GetComponent<Text>();
        comboTimerImage1 = GameObject.Find("comboTimer1").GetComponent<Image>();
        comboTimerImage2 = GameObject.Find("comboTimer2").GetComponent<Image>();

        livesUI = GameObject.Find("Lives").transform;
        scoreTextUI = GameObject.Find("Score").transform.Find("text").GetComponent<Text>();
    }

    private void Update() {
        comboTimer -= Time.deltaTime;
        comboTimerImage1.fillAmount = comboTimer / 3f;
        comboTimerImage2.fillAmount = comboTimer / 3f;

        if (comboTimer < 0f) {
            combo = 0;
            RefreshUI();
        }
    }

    public void RefreshUI() {
        for (int i = 0; i < Player.lives; i++) {
            livesUI.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = Player.lives; i < 3; i++) {
            livesUI.GetChild(i).gameObject.SetActive(false);
        }

        scoreTextUI.text = score.ToString();
        
        if (combo == 0) {
            comboUI.gameObject.SetActive(false);
        }
        else {
            comboUI.gameObject.SetActive(true);
        }

        comboTextUI.text = "X" + combo;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            combo++;
            comboTimer = comboTimerMax;
            score += 10 * combo;
            PlayerPrefs.SetInt("Score", score);
            Camera.main.GetComponent<CameraShake>().Shake();
            RefreshUI();
        }
    }
}
