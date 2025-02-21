using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Target : MonoBehaviour
{
    [SerializeField] private Types.ElementType elementType;
    [SerializeField] private Types.TargetType targetType;
    [SerializeField] private ParticleSystem explodeEffectPrefab;
    [SerializeField] private AudioClipsSO audioClipsSO;

    [SerializeField] private Transform[] miniTargets;

    private Rigidbody2D rigidBody2D;
    private PlayerStats player;

    private int[] increment;
    private int[] numberRange = { 2, 3 };
    private int[] damage;

    private float horizontalSpeedRange = 3;
    private float verticalSpeed;
    private float horizontalMoveLimit = 26f;
    private float verticalMoveLimit = 25f;
    private float force = 3f;

    private bool canSpawn;

    private const string HIGHSCORE = "highScore";


    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        int[] miniDamage = { 1, 2 };
        int[] normalDamage = { 3, 4 };
        int[] miniIncrement = { 2, 3 };
        int[] normalIncrement = { 5, 8 };
        if (targetType == Types.TargetType.mini)
        {
            damage = miniDamage;
            increment = miniIncrement;
        }
        else
        {
            damage = normalDamage;
            increment = normalIncrement;
        }
    }
    private void Start()
    {
        player = PlayerStats.Instance;
    }
    private void Update()
    {
        if (rigidBody2D.velocity.y != verticalSpeed && targetType == Types.TargetType.normal) rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, verticalSpeed);
        if (transform.position.x >= horizontalMoveLimit) rigidBody2D.AddForce(-new Vector2(force, 0), ForceMode2D.Impulse);
        if (transform.position.x <= -horizontalMoveLimit) rigidBody2D.AddForce(new Vector2(force, 0), ForceMode2D.Impulse);
        if (transform.position.y >= verticalMoveLimit) DestroySelf();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float volume = 1f;
        if (collision.gameObject.TryGetComponent(out Bullet bullet) != false)
        {
            if (bullet.GetBulletType() != elementType)
            {
                int score = player.GetScore() + Random.Range(increment[0], increment[1] + 1);
                player.SetScore(score);

                if (score > PlayerPrefs.GetInt(HIGHSCORE)) PlayerPrefs.SetInt(HIGHSCORE, score);

                if (canSpawn) SpawnMiniTargets();
                DestroySelf();

                AudioSource.PlayClipAtPoint(audioClipsSO.explode, transform.position, volume);
            }
            else
            {
                AudioSource.PlayClipAtPoint(audioClipsSO.hit, transform.position, volume);
            }
            bullet.DestroySelf();
        }
        else if (collision.gameObject.TryGetComponent(out Target target) == false)
        {
            if (player.GetGameState() == Types.GameState.gamePlaying)
            {
                int playerHP = player.GetHP() - Random.Range(damage[0], damage[1] + 1);
                player.SetHP(playerHP);
                if (player.GetHP() <= 0)
                {
                    player.SetHP(0);
                    player.SetGameState(Types.GameState.gameLost);
                }
            }
            AudioSource.PlayClipAtPoint(audioClipsSO.explode, transform.position, volume);
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        ParticleSystem explodeEffect = Instantiate(explodeEffectPrefab);
        explodeEffect.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        TargetSpawner.Instance.TargetDestroyed();
        Destroy(gameObject);
    }

    public void SetMoveVelocity(Vector2 moveVelocity)
    {
        verticalSpeed = moveVelocity.y;
        rigidBody2D.velocity = moveVelocity;
    }

    public void SetSpawnMini(bool spawnMini)
    {
        canSpawn = spawnMini;
    }

    public Types.ElementType GetTargetType()
    {
        return elementType;
    }

    private void SpawnMiniTargets()
    {
        if (targetType != Types.TargetType.mini)
        {
            int numberOfTargets = Random.Range(numberRange[0], numberRange[1] + 1);
            int chance = Random.Range(1, 4);

            int initialSign = Random.Range(1, 3);

            if (chance == 1)
            {
                for (int i = 0; i < numberOfTargets; i++)
                {
                    int targetIndex = Random.Range(0, miniTargets.Length);
                    Transform spawnedTarget = Instantiate(miniTargets[targetIndex]);
                    spawnedTarget.position = transform.position;

                    float horizontalSpeed;
                    float verticalSpeed = -Random.Range(rigidBody2D.velocity.magnitude * 1f, rigidBody2D.velocity.magnitude * 1.5f);
                    if (initialSign == 1)
                    {
                        if (i % 2 == 0) horizontalSpeed = Random.Range(0, horizontalSpeedRange);
                        else horizontalSpeed = Random.Range(-horizontalSpeedRange, 0);
                    }
                    else
                    {
                        if (i % 2 == 0) horizontalSpeed = Random.Range(-horizontalSpeedRange, 0);
                        else horizontalSpeed = Random.Range(0, horizontalSpeedRange);
                    }

                    Vector2 moveVelocity = new Vector2(horizontalSpeed, verticalSpeed);
                    spawnedTarget.GetComponent<Target>().SetMoveVelocity(moveVelocity);
                }
            }
        }
    }
}
