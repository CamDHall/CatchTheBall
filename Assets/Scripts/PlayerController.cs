using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float dashSpeed;

    // Variables for each player
    public string playerTeam;
    public string playerHAxis, playerVAxis, playerDashBtn;
    Vector2 Pos;

    // Components
    BoxCollider2D bCollider;
    public Rigidbody2D rb;

    // Flags
    bool wallColliding = false;
    public bool holdingBall;

	void Start () {
        holdingBall = false;

        playerHAxis = playerTeam + "Horizontal";
        playerVAxis = playerTeam + "Vertical";
        playerDashBtn = playerTeam + "Dash";
        Pos = transform.position;
        bCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
	}

    void FixedUpdate () {
        if (!wallColliding)
        {
            if(Input.GetButtonDown(playerDashBtn) && holdingBall)
            {
                BallController.Instance.ThrowBall(this.gameObject);
            }
            // If holding a times the axis by the dash speed
            if (Input.GetButton(playerDashBtn))
            {
                // Check if holding ball
                if (!holdingBall)
                {
                    Pos += new Vector2(Input.GetAxis(playerHAxis), Input.GetAxis(playerVAxis)) * dashSpeed;
                }
            } else
            {
                Pos += new Vector2(Input.GetAxis(playerHAxis), Input.GetAxis(playerVAxis));
            }

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
