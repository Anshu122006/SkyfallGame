using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] GameInputManager input;
    [SerializeField] private Transform[] bullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform playerBasePoint;
    [SerializeField] private AnimationClipsSO animationClipsSO;
    [SerializeField] private AudioClipsSO audioClipsSO;
    [SerializeField] private ParticleSystem explodeEffectPrefab;
    [SerializeField] private Camera mainCamera;

    private Animator animator;
    private bool canShoot = true;
    private float shootTime;
    private float delay = 0;
    private float moveLimit = 24f;
    private float moveSpeed = 11f;
    private float bulletSpeed = 12f;
    private float leftRotateLimit = -80;

    private Vector3 mousePos;
    private PlayerStats playerStats;
    private Types.ElementType elementType = Types.ElementType.ice;
    private Types.ControlType controlType;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
        input.OnShootKey += Input_OnShoot;
        GameScreenUI.Instance.OnSwitchControl += GameScreenUI_SwitchControls;
        animator = transform.GetChild(0).GetComponent<Animator>();
        shootTime = animationClipsSO.playerShoot.length + delay;
        controlType = Types.ControlType.move;
    }


    private void FixedUpdate()
    {
        if (playerStats.GetGameState() == Types.GameState.gamePlaying)
        {
            HandleMovement();
        }
    }

    private void GameScreenUI_SwitchControls(System.Object sender, EventArgs e)
    {
        if (controlType == Types.ControlType.move)
        {
            input.OnShootKey -= Input_OnShoot;
            input.OnShootMouse += Input_OnShoot;
            controlType = Types.ControlType.rotate;
        }
        else
        {
            input.OnShootMouse -= Input_OnShoot;
            input.OnShootKey += Input_OnShoot;
            controlType = Types.ControlType.move;
        }
        transform.GetChild(0).up = Vector3.up;
        transform.position = playerBasePoint.position;
    }

    private void FollowCursor()
    {
        Transform playerVisual = transform.GetChild(0);
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pointDirection = new Vector3(mousePos.x, mousePos.y, playerVisual.position.z) - playerVisual.position;
        float leftLimit = leftRotateLimit;
        float rightLimit = -leftRotateLimit;
        float currentAngle = Vector3.SignedAngle(pointDirection, Vector3.up, Vector3.forward);
        if (currentAngle >= leftLimit && currentAngle <= rightLimit) playerVisual.up = pointDirection;
    }

    private void HandleMovement()
    {
        switch (controlType)
        {
            case Types.ControlType.move:
                if (canShoot)
                {
                    if (transform.position.x < moveLimit && transform.position.x > -moveLimit)
                    {
                        float moveDistance = moveSpeed * Time.deltaTime;
                        Vector3 moveDir = new Vector3(input.GetMoveDirection().x, 0, 0);
                        transform.position += moveDir * moveDistance;
                    }
                    else if (transform.position.x >= moveLimit)
                    {
                        float moveDistance = moveSpeed * Time.deltaTime;
                        Vector3 moveDir = new Vector3(input.GetMoveDirection().x, 0, 0);
                        if (moveDir.x < 0) transform.position += moveDir * moveDistance;
                    }
                    else
                    {
                        float moveDistance = moveSpeed * Time.deltaTime;
                        Vector3 moveDir = new Vector3(input.GetMoveDirection().x, 0, 0);
                        if (moveDir.x > 0) transform.position += moveDir * moveDistance;
                    }
                }
                break;

            case Types.ControlType.rotate:
                if (canShoot)
                {
                    FollowCursor();
                }
                break;
        }

    }

    private void Input_OnShoot(object sender, EventArgs e)
    {
        if (playerStats.GetGameState() == Types.GameState.gamePlaying)
        {
            if (canShoot)
            {
                float volume = 1f;
                string SHOOT = "Shoot";
                string IS_IDLE = "IsIdle";
                animator.SetTrigger(SHOOT);
                animator.SetBool(IS_IDLE, false);
                canShoot = false;
                FunctionTimer.Create(() =>
                {
                    Transform shotBullet = null;
                    if (elementType == Types.ElementType.ice)
                    {
                        shotBullet = Instantiate(bullet[0]);
                        elementType = Types.ElementType.fire;
                    }
                    else
                    {
                        shotBullet = Instantiate(bullet[1]);
                        elementType = Types.ElementType.ice;
                    }
                    AudioSource.PlayClipAtPoint(audioClipsSO.shoot, transform.position, volume);
                    shotBullet.position = new Vector3(firePoint.position.x, firePoint.position.y, transform.position.z);
                    Vector2 bulletVelocity = transform.GetChild(0).up.normalized * bulletSpeed;
                    shotBullet.GetComponent<Bullet>().SetMoveVelocity(bulletVelocity);
                    animator.SetBool("IsIdle", true);
                    canShoot = true;
                }, shootTime);
            }
        }
    }
}
