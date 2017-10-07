using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public static BallController Instance;
    BoxCollider2D bCollider;

	void Start () {
        Instance = this;
        bCollider = GetComponent<BoxCollider2D>();
	}
	
	void Update () {
		
	}

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            transform.parent = col.gameObject.transform;
            transform.localPosition = Vector2.zero;
        }
    }
}
