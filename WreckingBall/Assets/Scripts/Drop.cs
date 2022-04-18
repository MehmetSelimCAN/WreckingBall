using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {

    private float moveTimer;
    private float moveTimerMax = 3f;

    private Vector2 direction;
    private Rigidbody2D rb;

    private Transform deathEffect;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        deathEffect = Resources.Load<Transform>("Prefabs/DeathEffects/pfDropDeathEffect");
    }

    private void Update() {
        moveTimer -= Time.deltaTime;
        if (moveTimer < 0f) {
            Move();
            moveTimer = moveTimerMax;
        }
    }

    private void Move() {
        float randomNumberX = Random.Range(-0.5f, 0.5f);
        float randomNumberY = Random.Range(-0.5f, 0.5f);
        direction = new Vector2(randomNumberX, randomNumberY);
        rb.velocity = direction;
    }

    private void Die() {
        Transform deathParticles = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathParticles.gameObject, 0.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "BallCollider") {
            Die();
        }
    }
}
