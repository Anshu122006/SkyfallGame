using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Target_dummy : MonoBehaviour
{
    [SerializeField] private ParticleSystem explodeEffectPrefab;
    [SerializeField] private AudioClipsSO audioClipsSO;
    private Rigidbody2D rigidBody2D;
    private float moveSpeed;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 moveVector = new Vector2(0, -1) * moveSpeed;
        if (rigidBody2D.velocity != moveVector) rigidBody2D.velocity = moveVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float volume = 1f;
        AudioSource.PlayClipAtPoint(audioClipsSO.explode, transform.position, volume);
        DestroySelf();
    }

    public void DestroySelf()
    {
        ParticleSystem explodeEffect = Instantiate(explodeEffectPrefab);
        explodeEffect.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        Destroy(gameObject);
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
}
