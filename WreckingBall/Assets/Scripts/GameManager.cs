using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private Button playButton;
    //private Button versusButton;

    //private Transform arcadeTilemap;
    //private Transform versusTilemap;

    private static bool gameOver;
    private static Transform gameScreen;
    private static Transform gameOverScreen;
    private static Transform notHighScored;
    private static Transform highScored;
    private static Transform comboUI;

    private static Transform menuScreen;

    private Transform playerPrefab;
    private Transform ballPrefab;


    private void Awake() {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        //versusButton = GameObject.Find("VersusButton").GetComponent<Button>();

        //arcadeTilemap = GameObject.Find("ArcadeBorder").transform;
        //versusTilemap = GameObject.Find("VersusBorder").transform;
        //versusTilemap.gameObject.SetActive(false);

        playButton.onClick.AddListener(() => {
            Play();
        });

        /*versusButton.onClick.AddListener(() => {
            Versus();
        });*/

        gameScreen = GameObject.Find("GameScreen").transform;
        gameOverScreen = GameObject.Find("GameOverScreen").transform;
        notHighScored = GameObject.Find("notHighScored").transform;
        highScored = GameObject.Find("HighScored").transform;
        comboUI = GameObject.Find("Combo").transform;

        menuScreen = GameObject.Find("MenuScreen").transform;
        menuScreen.Find("highScoreText").GetComponent<Text>().text = "high score : " + PlayerPrefs.GetInt("HighScore");

        gameScreen.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        notHighScored.gameObject.SetActive(false);
        highScored.gameObject.SetActive(false);

        menuScreen.gameObject.SetActive(true);
        playerPrefab = Resources.Load<Transform>("Prefabs/pfPlayer");
        ballPrefab = Resources.Load<Transform>("Prefabs/pfBall");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            BackToMenu();
        }
        if (gameOver && Input.GetKeyDown(KeyCode.Space)) {
            Restart();
        }
    }

    public static void GameOver() {
        gameOver = true;
        gameScreen.gameObject.SetActive(false);

        if (PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("HighScore")) {
            PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("Score"));
            gameOverScreen.gameObject.SetActive(true);
            highScored.gameObject.SetActive(true);
            notHighScored.gameObject.SetActive(false);
            highScored.Find("yourScore").GetComponent<Text>().text = PlayerPrefs.GetInt("Score").ToString();
            menuScreen.Find("highScoreText").GetComponent<Text>().text = "high score : " + PlayerPrefs.GetInt("HighScore");
        }
        else {
            gameOverScreen.gameObject.SetActive(true);
            highScored.gameObject.SetActive(false);
            notHighScored.gameObject.SetActive(true);
            notHighScored.Find("highScore").GetComponent<Text>().text = "high score\n" + PlayerPrefs.GetInt("HighScore");
            notHighScored.Find("yourScore").GetComponent<Text>().text = "your score \n" + PlayerPrefs.GetInt("Score");
        }
    }

    private void Play() {
        //versusTilemap.gameObject.SetActive(false);
        //arcadeTilemap.gameObject.SetActive(true);

        menuScreen.gameObject.SetActive(false);
        gameScreen.gameObject.SetActive(true);
        comboUI.gameObject.SetActive(true);
        Instantiate(ballPrefab, new Vector3(3f,-2.5f,0f), Quaternion.identity);
        Instantiate(playerPrefab, new Vector3(1f,-2.5f,0f), Quaternion.identity);
    }

    /*private void Versus() {
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject bug in bugs) {
            Destroy(bug);
        }
        versusTilemap.gameObject.SetActive(true);
        arcadeTilemap.gameObject.SetActive(false);

        menuScreen.gameObject.SetActive(false);
        gameScreen.gameObject.SetActive(true);
        comboUI.gameObject.SetActive(true);
        Instantiate(ballPrefab, new Vector3(2f, 0f, 0f), Quaternion.identity);
        Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }*/

    private void BackToMenu() {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("Ball"));
        GameObject.Find("CameraHolder").transform.position = new Vector3(1f, -2.5f, 0f);
        gameOver = false;

        GameObject[] bugs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject bug in bugs) {
            Destroy(bug);
        }

        for (int i = 0; i < 8; i++) {
            EnemySpawner.Instance.SpawnSmallBug();
        }

        menuScreen.gameObject.SetActive(true);
        gameOverScreen.gameObject.SetActive(false);
    }

    private void Restart() {
        gameOver = false;
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject bug in bugs) {
            Destroy(bug);
        }

        for (int i = 0; i < 8; i++) {
            EnemySpawner.Instance.SpawnSmallBug();
        }

        gameOverScreen.gameObject.SetActive(false);

        Play();
    }
}
