using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Rigidbody2D rb;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sprite flySprite;
    private Sprite frogSprite;
    private Sprite spiderSprite;
    private Sprite basicSprite;

    private Vector2 direction;

    private float flySpeed = 4f;
    private Vector2 flyTarget;
    private float flyMoveTimer;
    private float flyMoveTimerMax = 0.2f;

    private float frogMoveTimer;
    private float frogMoveTimerMax = 2f;

    private float spiderMoveTimer;
    private float spiderMoveTimerMax = 1.5f;

    private float basicMoveTimer;
    private float basicMoveTimerMax = 3f;

    private float bigFireflyMoveTimer;
    private float bigFireflyMoveTimerMax = 3f;
    private float bigFireflyEggTimer;
    private float bigFireflyEggTimerMax = 3.5f;
    private Transform bigFireflyEgg;

    private Transform sittingBugEgg;

    private ParticleSystem deathEffect;
    public static int combo;


    [SerializeField] public EnemyType enemyType;

    public enum EnemyType {
        Frog,
        Spider,
        Fly,
        Basic,
        BigFirefly,
        SittingBug
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        flySprite = Resources.Load<Sprite>("Sprites/spFlySprite");
        frogSprite = Resources.Load<Sprite>("Sprites/spFrogSprite");
        spiderSprite = Resources.Load<Sprite>("Sprites/spSpiderSprite");
        basicSprite = Resources.Load<Sprite>("Sprites/spBasicSprite");

        bigFireflyEgg = Resources.Load<Transform>("Prefabs/pfBigFireflyEgg");
        sittingBugEgg = Resources.Load<Transform>("Prefabs/pfSittingBugEgg");

        /*animator = GetComponent<Animator>();

        

        if (enemyType == EnemyType.Fly) {
            spriteRenderer.sprite = flySprite;
            animator.Play("Fly");
        }

        if (enemyType == EnemyType.Frog) {
            spriteRenderer.sprite = frogSprite;
            animator.Play("Frog");
        }

        if (enemyType == EnemyType.Spider) {
            spriteRenderer.sprite = spiderSprite;
            animator.Play("Spider");
        }

        if (enemyType == EnemyType.Basic) {
            spriteRenderer.sprite = basicSprite;
            animator.Play("Basic");
        }*/
    }

    private void Update() {
        if (enemyType == EnemyType.Fly) {
            flyMoveTimer -= Time.deltaTime;

            if (flyMoveTimer < 0f) {
                FlyMove();
                flyMoveTimer = flyMoveTimerMax;
            }
        }

        if (enemyType == EnemyType.Frog) {
            frogMoveTimer -= Time.deltaTime;
            if (frogMoveTimer < 0f) {
                FrogMove();
                frogMoveTimer = frogMoveTimerMax;
            }
            rb.velocity = direction * frogMoveTimer;
        }

        if (enemyType == EnemyType.Spider) {
            spiderMoveTimer -= Time.deltaTime;
            if (spiderMoveTimer < 0f) {
                SpiderMove();
                spiderMoveTimer = spiderMoveTimerMax;
            }
            rb.velocity = direction * spiderMoveTimer;
        }

        if (enemyType == EnemyType.Basic) {
            basicMoveTimer -= Time.deltaTime;
            if (basicMoveTimer < 0f) {
                BasicMove();
                basicMoveTimer = basicMoveTimerMax;
            }
        }

        if (enemyType == EnemyType.BigFirefly) {
            bigFireflyMoveTimer -= Time.deltaTime;
            bigFireflyEggTimer -= Time.deltaTime;
            if (bigFireflyMoveTimer < 0f) {
                BigFireflyMove();
                bigFireflyMoveTimer = bigFireflyMoveTimerMax;
            }
            rb.velocity = direction * bigFireflyMoveTimer;
            
            if (bigFireflyEggTimer < 0f) {
                Transform egg = Instantiate(bigFireflyEgg, transform.position, Quaternion.identity);
                egg.GetComponent<Rigidbody2D>().AddForce(-rb.velocity * 100f);
                bigFireflyEggTimer = bigFireflyEggTimerMax;
            }


        }

    }

    private void FlyMove() {
        float randomNumberX = Random.Range(-1f,1f);
        float randomNumberY = Random.Range(-1f,1f);
        flyTarget = new Vector3(transform.position.x + randomNumberX, transform.position.y + randomNumberY, 0f);
        direction = flyTarget - (Vector2)transform.position;
        rb.velocity = direction.normalized * flySpeed;
    }

    private void FrogMove() {
        float randomNumberX = Random.Range(-3f, 3f);
        float randomNumberY = Random.Range(-3f, 3f);
        direction = new Vector2(randomNumberX,randomNumberY);
    }

    private void SpiderMove() {
        float randomNumberX = Random.Range(-1.5f, 1.5f);
        float randomNumberY = Random.Range(-1.5f, 1.5f);
        direction = new Vector2(randomNumberX, randomNumberY);
    }

    private void BasicMove() {
        float randomNumberX = Random.Range(-0.5f, 0.5f);
        float randomNumberY = Random.Range(-0.5f, 0.5f);
        direction = new Vector2(randomNumberX, randomNumberY);
        rb.velocity = direction;
    }

    private void BigFireflyMove() {
        float randomNumberX = Random.Range(-3f, 3f);
        float randomNumberY = Random.Range(-3f, 3f);
        direction = new Vector2(randomNumberX, randomNumberY);
    }

    private void SittingBug() {

    }

    private IEnumerator Die() {
        if (enemyType == EnemyType.SittingBug) {
            for (int i = 0; i < 8; i++) {
                Vector3 direction = transform.Find("sittingBugSpawnPoints").GetChild(i).transform.position - transform.position;
                Transform egg = Instantiate(sittingBugEgg, transform.Find("sittingBugSpawnPoints").GetChild(i).transform.position, Quaternion.identity);
                egg.GetComponent<Rigidbody2D>().AddForce(direction * 1000f);
            }
        }

        if (combo == 0) {
            Ball.score += 5;
        }
        else {
            Ball.score += 10 * combo;
        }
        //deathEffect.Play();
        this.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && !Player.Instance.HasGhostMode()) {
            Player.Instance.DieFromAnotherScript();
        }

        if (collision.tag == "Ball") {
            StartCoroutine(Die());
        }
    }
}
