
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : RecyclableComponent
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private Vector2 velocity;
    private float timeAlive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SpawnBullet(Vector2 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        timeAlive = 0;
        velocity = transform.right * speed;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > lifeTime)
        {
            this.Recycle();
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }
}
