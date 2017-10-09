using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public static BallController Instance;
    BoxCollider2D bCollider;

    bool rebounding;

    public float throwForce;
    public float decelerationSpeed;
    float force, decSpeed;


    Vector3 directionOfBall; // For throwing

    bool throwingBall = false, moving = false;

	void Start () {
        Instance = this;
        bCollider = GetComponent<BoxCollider2D>();
	}

    private void FixedUpdate()
    {
        // not held
        if (transform.parent == null)
        {
            Vector2 pt1 = transform.TransformPoint(bCollider.offset + bCollider.size);//(box.size / 2));
            Vector2 pt2 = transform.TransformPoint(bCollider.offset - (bCollider.size) + new Vector2(0, 0));
            if (Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Wall")))
            {
                if (!rebounding)
                {
                    Debug.Log("WALL");
                    StartCoroutine("Rebound");
                    moving = true;
                    rebounding = true;
                }
            }

            if (force > 0)
            {
                if (moving)
                {
                    decSpeed = (decSpeed * Time.deltaTime) + (Time.deltaTime / 5);
                    force -= decSpeed;
                    transform.Translate(directionOfBall * force);
                }
            }
            else
            {
                moving = false;
            }
        }

    }

    public void ThrowBall(GameObject thrower)
    {
        moving = true;
        force = throwForce;
        decSpeed = decelerationSpeed;
        transform.SetParent(null);
        throwingBall = true;

        thrower.GetComponent<PlayerController>().holdingBall = false;

        float x, y;

        // check pos or neg 
        if (Input.GetAxis(thrower.GetComponent<PlayerController>().playerHAxis) < 0)
            x = -1;
        else if (Input.GetAxis(thrower.GetComponent<PlayerController>().playerHAxis) == 0)
            x = 0;
        else
            x = 1;

        if (Input.GetAxis(thrower.GetComponent<PlayerController>().playerVAxis) < 0)
            y = -1;
        else if (Input.GetAxis(thrower.GetComponent<PlayerController>().playerVAxis) == 0)
            y = 0;
        else
            y = 1;

        // Check for diag
        if (x != 0 && y != 0)
        {
            x *= 0.5f;
            y *= 0.5f;
        }

        directionOfBall = new Vector3(x, y, 0);

        thrower.GetComponent<PlayerController>().holdingBall = false;
        StartCoroutine("BallThrown");
    }

    IEnumerator BallThrown()
    {
        yield return new WaitForSeconds(0.25f);

        throwingBall = false;
    }

    IEnumerator Rebound()
    {
        directionOfBall *= -1;
        throwingBall = false;
        force /= 2;

        yield return new WaitForSeconds(Time.deltaTime);
        rebounding = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player" && !throwingBall)
        {
            moving = false;
            transform.parent = col.gameObject.transform;
            transform.localPosition = Vector2.zero;

            col.gameObject.GetComponent<PlayerController>().holdingBall = true;
        }
    }
}
