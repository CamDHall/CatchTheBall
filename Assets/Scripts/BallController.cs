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
        Vector2 pt1 = transform.TransformPoint(bCollider.offset + new Vector2(bCollider.size.x / 2, -bCollider.size.y / 2));
        Vector2 pt2 = transform.TransformPoint(bCollider.offset - (bCollider.size / 2) + new Vector2(0, 0));

        if(Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Player")) != null)
        {
            transform.parent = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Player")).gameObject.transform;
            transform.position = Vector2.Lerp(transform.position, Vector2.zero, Time.deltaTime);
        }
    }
}
