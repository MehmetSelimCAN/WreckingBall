using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBug : MonoBehaviour {

    private Transform redBugEgg;
    private Transform deathEffect;

    private void Awake() {
        redBugEgg = Resources.Load<Transform>("Prefabs/Bugs/pfRedBugEgg");
        deathEffect = Resources.Load<Transform>("Prefabs/DeathEffects/pfRedBugDeathEffect");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            Die();
        }
    }

    private void Die() {
        for (int i = 0; i < 4; i++) {
            Vector3 direction = transform.Find("eggSpawnPoints").GetChild(i).transform.position - transform.position;
            Transform egg = Instantiate(redBugEgg, transform.Find("eggSpawnPoints").GetChild(i).transform.position, Quaternion.identity);
            egg.GetComponent<Rigidbody2D>().AddForce(direction * 500f);
        }
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
