using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float dashSpeed;

    // Variables for each player
    public string playerTeam;
    public string playerHAxis, playerVAxis, playerDashBtn;
    Vector2 Pos;

    bool inEnemyGoal = false;
    public float debuffSpeed;
    float speedDebuff;

    // Components
    BoxCollider2D bCollider;
    public Rigidbody2D rb;

    // Flags
    bool wallColliding = false;
    public bool holdingBall;

    public float tagBackTimer = 0;

	void Start () {
        holdingBall = false;

        playerHAxis = playerTeam + "Horizontal";
        playerVAxis = playerTeam + "Vertical";
        playerDashBtn = playerTeam + "Dash";
        Pos = transform.position;
        bCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        if (Input.GetButtonDown(playerDashBtn) && holdingBall)
        {
            BallController.Instance.ThrowBall(this.gameObject);
        }
    }

    void FixedUpdate () {
        // Slow speed inside enemy goal
        if (playerTeam == "Player1")
        {
            Vector2 pt1 = transform.TransformPoint(bCollider.offset + new Vector2(bCollider.size.x / 2, -bCollider.size.y / 2));//(box.size / 2));
            Vector2 pt2 = transform.TransformPoint(bCollider.offset - (bCollider.size / 2) + new Vector2(0, 0));
            inEnemyGoal = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("RightGoal")) != null;

            if (inEnemyGoal)
                speedDebuff = debuffSpeed;
            else
                speedDebuff = 1;
        } else
        {
            Vector2 pt1 = transform.TransformPoint(bCollider.offset + new Vector2(bCollider.size.x / 2, -bCollider.size.y / 2));//(box.size / 2));
            Vector2 pt2 = transform.TransformPoint(bCollider.offset - (bCollider.size / 2) + new Vector2(0, 0));
            inEnemyGoal = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("LeftGoal")) != null;

            if (inEnemyGoal)
                speedDebuff = debuffSpeed;
            else
                speedDebuff = 1;
        }

        // Change score
        if (holdingBall)
        {
            if (playerTeam == "Player1")
                GameManager.Instance.leftTeamScore += Time.deltaTime * GameManager.Instance.scoringValue;
            else
                GameManager.Instance.rightTeamScore += Time.deltaTime * GameManager.Instance.scoringValue;
        }

        if (!wallColliding)
        {
            // If holding a times the axis by the dash speed
            if (Input.GetButton(playerDashBtn))
            {
                // Check if holding ball
                if (!holdingBall)
                {
                    Pos += new Vector2(Input.GetAxis(playerHAxis) * speedDebuff, speedDebuff * Input.GetAxis(playerVAxis)) * dashSpeed;
                }
            } else
            {
                Pos += new Vector2(Input.GetAxis(playerHAxis) * speedDebuff, Input.GetAxis(playerVAxis) * speedDebuff);
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

        if(col.gameObject.tag == "Player" && col.gameObject.transform.childCount > 0)
        {
            Debug.Log(col.gameObject);
            if (tagBackTimer <= Time.time)
            {
                col.gameObject.GetComponent<PlayerController>().tagBackTimer = Time.time + 3f;
                BallController.Instance.ParentBall(col.gameObject, this.gameObject);
            }
        }
    }
}
