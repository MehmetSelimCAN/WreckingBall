using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour {

    private float moveTimer;
    private float moveTimerMax = 1.5f;

    private Vector2 direction;
    private Rigidbody2D rb;

    private Transform deathEffect;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        deathEffect = Resources.Load<Transform>("Prefabs/DeathEffects/pfSpiderDeathEffect");
    }

    private void Update() {
        moveTimer -= Time.deltaTime;
        if (moveTimer < 0f) {
            Move();
            moveTimer = moveTimerMax;
        }
        rb.velocity = direction * moveTimer;
    }

    private void Move() {
        float randomNumberX = Random.Range(-1.5f, 1.5f);
        float randomNumberY = Random.Range(-1.5f, 1.5f);
        direction = new Vector2(randomNumberX, randomNumberY);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
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
