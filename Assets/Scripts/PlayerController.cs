using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float dashSpeed;

    public ParticleSystem ps;

    // Variables for each player
    public string playerTeam;
    public string playerHAxis, playerVAxis, playerDashBtn;
    Vector2 Pos;

    bool inOwnGoal = false;
    public float debuffSpeed;
    float speedDebuff;

    // Components
    BoxCollider2D bCollider;
    public Rigidbody2D rb;

    // Flags
    bool wallColliding = false;
    public bool holdingBall;

    public bool stunned = false;
    public float stunnedTimer;

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

        // Check stun state
        if (stunnedTimer < Time.time)
            stunned = false;
    }

    void FixedUpdate () {
        // Slow speed inside enemy goal
        if (playerTeam == "Player1" || playerTeam == "Player2")
        {
            Vector2 pt1 = transform.TransformPoint(bCollider.offset + new Vector2(bCollider.size.x / 2, -bCollider.size.y / 2));//(box.size / 2));
            Vector2 pt2 = transform.TransformPoint(bCollider.offset - (bCollider.size / 2) + new Vector2(0, 0));
            inOwnGoal = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("LeftGoal")) != null;

            if (inOwnGoal)
                speedDebuff = debuffSpeed;
            else
                speedDebuff = 1;
        } else
        {
            Vector2 pt1 = transform.TransformPoint(bCollider.offset + new Vector2(bCollider.size.x / 2, -bCollider.size.y / 2));//(box.size / 2));
            Vector2 pt2 = transform.TransformPoint(bCollider.offset - (bCollider.size / 2) + new Vector2(0, 0));
            inOwnGoal = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("RightGoal")) != null;

            if (inOwnGoal)
                speedDebuff = debuffSpeed;
            else
                speedDebuff = 1;
        }

        // Change score
        if (holdingBall)
        {
            if (playerTeam == "Player1" || playerTeam == "Player2")
                GameManager.Instance.leftTeamScore += Time.deltaTime * GameManager.Instance.scoringValue;
            else
                GameManager.Instance.rightTeamScore += Time.deltaTime * GameManager.Instance.scoringValue;
        }

        if (!wallColliding && !stunned)
        {
            // If holding a times the axis by the dash speed
            if (Input.GetButton(playerDashBtn))
            {
                // Check if holding ball
                if (!holdingBall)
                {
                    Pos += new Vector2(Input.GetAxis(playerHAxis), Input.GetAxis(playerVAxis)) * speedDebuff * dashSpeed;
                } else
                {
                    Pos += new Vector2(Input.GetAxis(playerHAxis), Input.GetAxis(playerVAxis) * speedDebuff);
                }
            } else
            {
                Pos += new Vector2(Input.GetAxis(playerHAxis), Input.GetAxis(playerVAxis) * speedDebuff);
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Wall")
        {
            StartCoroutine("BouncePlayer");
            Instantiate(ps, transform.position, Quaternion.identity);

            ps.startColor = Color.grey;

            wallColliding = true;
        }

        if (col.gameObject.tag == "Player" && col.gameObject.transform.childCount > 0)
        {
            PlayerController pc = col.gameObject.GetComponent<PlayerController>();
            if (tagBackTimer <= Time.time)
            {
                col.gameObject.GetComponent<PlayerController>().tagBackTimer = Time.time + 1.5f;
                BallController.Instance.ParentBall(col.gameObject, this.gameObject);
                Instantiate(ps,transform.position, Quaternion.identity);

                ps.startColor = GetComponent<SpriteRenderer>().color;

                // Stun player
                if (!pc.stunned)
                {
                    pc.stunned = true;
                    pc.stunnedTimer = Time.time + 1f;
                }
            }
        }
    }
}
