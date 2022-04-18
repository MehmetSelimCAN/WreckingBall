using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {

    private float speed = 4f;
    private Vector2 target;
    private float moveTimer;
    private float moveTimerMax = 0.2f;

    private Vector2 direction;
    private Rigidbody2D rb;
    private Transform deathEffect;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        deathEffect = Resources.Load<Transform>("Prefabs/DeathEffects/pfFlyDeathEffect");
    }

    private void Update() {
        moveTimer -= Time.deltaTime;

        if (moveTimer < 0f) {
            Move();
            moveTimer = moveTimerMax;
        }
    }

    private void Move() {
        float randomNumberX = Random.Range(-1f, 1f);
        float randomNumberY = Random.Range(-1f, 1f);
        target = new Vector3(transform.position.x + randomNumberX, transform.position.y + randomNumberY, 0f);
        direction = target - (Vector2)transform.position;
        rb.velocity = direction.normalized * speed;
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
