using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBugEgg : MonoBehaviour {

    private Vector3 scale;
    private CircleCollider2D circleCollider;

    private void Awake() {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start() {
        scale = transform.localScale;
        Destroy(gameObject, 2f);
    }

    private void Update() {
        if (scale.x > 0) {
            scale.x -= 0.0005f;
            scale.y -= 0.0005f;
            circleCollider.radius -= 0.0005f;
        }
        transform.localScale = scale;
    }
}
