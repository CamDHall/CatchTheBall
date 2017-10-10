using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public GameObject leftGoal, rightGoal;

    public float scoringValue;
    public float leftTeamScore, rightTeamScore;
    bool gameOver = false;

    public GameObject winScreen;

    void Start () {
        Instance = this;
        winScreen.SetActive(false);
	}
	
	void Update () {
        if (!gameOver)
        {
            Vector2 leftPos = new Vector2(leftTeamScore - 12.8f, 0);
            Vector2 rightPos = new Vector2(-rightTeamScore + 12.8f, 0);

            leftGoal.transform.position = leftPos;
            rightGoal.transform.position = rightPos;
        }

        if (leftGoal.transform.position.x > -4.4f)
        {
            gameOver = true;
            winScreen.SetActive(true);
            winScreen.GetComponentInChildren<Text>().text = "Blue Team Wins!";
        }

        if (rightGoal.transform.position.x < 4.4f)
        {
            gameOver = true;
            winScreen.SetActive(true);
            winScreen.GetComponentInChildren<Text>().text = "Red Team Wins!";
        }
    }

    public void Reload()
    {
        SceneManager.LoadScene("main");
    }
}
