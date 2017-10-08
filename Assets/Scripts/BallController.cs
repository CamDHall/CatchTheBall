using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public static BallController Instance;
    BoxCollider2D bCollider;

    public float throwForce;
    public float forceDeceleration;
    float remainingForce;

    Vector3 directionOfBall; // For throwing

    bool throwingBall = false;

	void Start () {
        Instance = this;
        bCollider = GetComponent<BoxCollider2D>();
	}

    private void FixedUpdate()
    {
        if (throwingBall)
        {
            Vector2 pt1 = transform.TransformPoint(bCollider.offset + bCollider.size);//(box.size / 2));
            Vector2 pt2 = transform.TransformPoint(bCollider.offset - (bCollider.size) + new Vector2(0, 0));
            if (Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Wall")))
            {
                throwingBall = false;
            } else
            {
                if(remainingForce > 0)
                {
                    remainingForce -= Time.deltaTime * forceDeceleration;
                    transform.Translate(directionOfBall * remainingForce);
                }
            }
        }

    }

    public void ThrowBall(GameObject thrower)
    {
        transform.SetParent(null);
        throwingBall = true;

        remainingForce = throwForce; // Reset used force var

        thrower.GetComponent<PlayerController>().holdingBall = false;

        int x, y;

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

        directionOfBall = new Vector3(x, y, 0);

        thrower.GetComponent<PlayerController>().holdingBall = false;
        StartCoroutine("BallThrown");
    }

    IEnumerator BallThrown()
    {
        yield return new WaitForSeconds(0.25f);

        throwingBall = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player" && !throwingBall)
        {

            transform.parent = col.gameObject.transform;
            transform.localPosition = Vector2.zero;

            col.gameObject.GetComponent<PlayerController>().holdingBall = true;
        }
    }
}
