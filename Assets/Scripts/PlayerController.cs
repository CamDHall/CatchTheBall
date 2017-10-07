using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public string playerTeam;
    public int speed;
    string playerHAxis;
    string playerVAxis;
    Vector2 Pos;
    BoxCollider2D bCollider;
    Rigidbody2D rb;
    bool wallColliding = false;

	void Start () {
        playerHAxis = playerTeam + "Horizontal";
        playerVAxis = playerTeam + "Vertical";
        Pos = transform.position;
        bCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        if (!wallColliding)
        {
            Pos += new Vector2(Input.GetAxis(playerHAxis), Input.GetAxis(playerVAxis));
            rb.MovePosition(Pos);
        }
    }

    IEnumerator BouncePlayer()
    {
        rb.velocity = new Vector2(-Pos.x, -Pos.y) * 1.1f;
        yield return new WaitForSeconds(0.25f);

        rb.velocity = Vector2.zero;
        Pos = transform.position;
        wallColliding = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Wall")
        {
            StartCoroutine("BouncePlayer");
            wallColliding = true;
        }
    }
}
