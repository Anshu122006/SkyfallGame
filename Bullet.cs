using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Types.ElementType bulletType;
    [SerializeField] private ParticleSystem explodeEffectPrefab;
    private Rigidbody2D rigidBody2D;
    private float moveSpeed;
    Transform destroyPoint;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        destroyPoint = DestroyPoint.Instance.GetDestroyPoint(1);
    }
    private void Update()
    {
        if (rigidBody2D.velocity.magnitude != moveSpeed) rigidBody2D.velocity = rigidBody2D.velocity.normalized * moveSpeed;
        if (transform.position.y >= destroyPoint.position.y) DestroySelf();
    }

    public void DestroySelf()
    {
        ParticleSystem explodeEffect = Instantiate(explodeEffectPrefab);
        explodeEffect.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        Destroy(gameObject);
    }

    public void SetMoveVelocity(Vector2 moveVelocity)
    {
        moveSpeed = moveVelocity.magnitude;
        rigidBody2D.velocity = moveVelocity;

    }
    public Types.ElementType GetBulletType()
    {
        return bulletType;
    }
}
