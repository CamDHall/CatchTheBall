using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public string playerTeam;
    public int speed;
    string playerHAxis;
    string playerVAxis;
    Vector2 Pos;

	void Start () {
        playerHAxis = playerTeam + "Horizontal";
        playerVAxis = playerTeam + "Vertical";
        Pos = transform.position;
	}
	
	void FixedUpdate () {
        // Replace
        //
        //
        /*
		if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * (speed * Time.deltaTime));
        }

        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * (speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * (speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * (speed * Time.deltaTime));
        }
        */

        Debug.Log(Input.GetAxis(playerHAxis));

        Pos += new Vector2(Input.GetAxis(playerHAxis), Input.GetAxis(playerVAxis));
        GetComponent<Rigidbody2D>().MovePosition(Pos);
    }
}
