using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private Transform startPoint;
    private Transform endPoint;
    [SerializeField] private Transform cloud;

    private float halfWindowWidth = 22f;
    private float moveSpeed = 5f;

    private bool hasSpwned;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        startPoint = transform.GetChild(0);
        endPoint = transform.GetChild(1);
        rigidBody2D.velocity = new Vector2(-moveSpeed, 0);
    }

    private void Update()
    {
        if (startPoint.position.x <= -halfWindowWidth && !hasSpwned)
        {
            Transform newCloud = Instantiate(cloud);
            newCloud.position = new Vector2(endPoint.position.x + halfWindowWidth * 2.5f, 0);
            hasSpwned = true;
        }
        if (endPoint.position.x <= -halfWindowWidth)
        {
            Destroy(gameObject);
        }
    }

}
