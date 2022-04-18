using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public static Player Instance { private set; get; }

    private LineRenderer lineRenderer;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSprite;
    private Vector2 movement;
    private float movementSpeed = 25f;

    public static int lives = 3;
    private Transform spawnPoint;
    private ParticleSystem deathEffect;

    private bool ghostMode;

    private Transform ball;
    private DistanceJoint2D djBall;
    private Rigidbody2D rbBall;
    private Transform ballPointer;

    private Color matColor = new Color(1f,1f,1f,1f);
    private Color transparentColor = new Color(1f,1f,1f,0.5f);

    private Transform cameraHolder;

    private void Awake() {
        Instance = this;

        lineRenderer = GetComponentInChildren<LineRenderer>();
        playerRb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        deathEffect = GetComponentInChildren<ParticleSystem>();

        ball = GameObject.FindGameObjectWithTag("Ball").transform;
        djBall = ball.GetComponent<DistanceJoint2D>();
        rbBall = ball.GetComponent<Rigidbody2D>();
        ball.GetComponent<DistanceJoint2D>().connectedBody = playerRb;

        ballPointer = GameObject.Find("ballPointer").transform;
        ballPointer.gameObject.SetActive(false);

        spawnPoint = GameObject.Find("spawnPoint").transform;

        cameraHolder = GameObject.Find("CameraHolder").transform;

        Ball.score = 0;
        lives = 3;
    }

    private void Start() {
        StartCoroutine(GhostMode());
    }

    private void Update() {
        BallPointerMovement();
        LineRendererMovement();
        CameraMovement();
        ball.transform.Rotate(new Vector3(0f ,0f ,rbBall.velocity.magnitude / 3f));

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (djBall.enabled) {
                djBall.enabled = false;
                lineRenderer.enabled = false;
                ballPointer.gameObject.SetActive(true);
                if (rbBall.velocity.magnitude < 10) {
                    rbBall.velocity = rbBall.velocity.normalized;
                    rbBall.velocity *= 15f;
                }
            }

            else if (!djBall.enabled) {
                djBall.enabled = true;
                lineRenderer.enabled = true;
                ballPointer.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate() {
        #region Player Movement
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        movement = Vector2.ClampMagnitude(movement, 1f);
        playerRb.velocity = movement * movementSpeed;
        #endregion 
    }


    private IEnumerator GhostMode() {
        ghostMode = true;

        #region Ghost Animation
        for (int i = 0; i < 6; i++) {
            playerSprite.color = transparentColor;
            lineRenderer.material.color = transparentColor;
            ball.GetComponent<SpriteRenderer>().color = transparentColor;
            yield return new WaitForSeconds(0.25f);
            playerSprite.color = matColor;
            lineRenderer.material.color = matColor;
            ball.GetComponent<SpriteRenderer>().color = matColor;
            yield return new WaitForSeconds(0.25f);
        }
        #endregion

        ghostMode = false;
    }

    public bool HasGhostMode() {
        return ghostMode;
    }

#region Linerenderer, camera and ball pointer movement
    private void LineRendererMovement() {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, ball.position);
    }

    private void BallPointerMovement() {
        Vector2 boundedArea = ball.position - transform.position;
        boundedArea.Normalize();
        boundedArea = Vector2.ClampMagnitude(boundedArea, 1f);
        float angle = AngleBetweenTwoPoints(ballPointer.position, ball.position);

        ballPointer.position = new Vector3(transform.position.x + boundedArea.x, transform.position.y + boundedArea.y, 0f);
        ballPointer.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void CameraMovement() {
        Vector3 newCameraPosition = new Vector3(transform.position.x, transform.position.y, -10f);
        cameraHolder.position = newCameraPosition;
    }
    #endregion

    public void DieFromAnotherScript() {
        StartCoroutine(Die());
    }

    private IEnumerator Die() {
        lives--;
        Ball.Instance.RefreshUI();
        deathEffect.Play();

        GetComponent<BoxCollider2D>().enabled = false;
        playerSprite.enabled = false;
        playerRb.velocity = Vector2.zero;
        ballPointer.gameObject.SetActive(false);
        lineRenderer.enabled = false;
        Ball.combo = 0;
        Ball.Instance.RefreshUI();
        ball.gameObject.SetActive(false);
        this.enabled = false;

        yield return new WaitForSeconds(2.5f);
        if (lives < 1) {
            GameManager.GameOver();
            Destroy(gameObject);
            Destroy(ball.gameObject);
        }
        else {
            Spawn();
        }
    }

    private void Spawn() {
        this.enabled = true;
        StartCoroutine(GhostMode());
        transform.position = spawnPoint.position;
        playerSprite.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        ball.position = spawnPoint.position + new Vector3(2f,0f,0f);
        ball.gameObject.SetActive(true);
        djBall.enabled = true;
        lineRenderer.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if ((collision.collider.tag == "Ball" || collision.collider.tag == "Enemy" || collision.collider.tag == "Egg") && !ghostMode) {
            StartCoroutine(Die());
        }
    }

}
