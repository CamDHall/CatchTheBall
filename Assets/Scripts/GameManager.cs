using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public GameObject player1, player2, player3, player4;
    public Color[] teamColors = new Color[2];


    void Start () {
        Instance = this;
	}
	
	void Update () {
		
	}
}
