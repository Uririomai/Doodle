using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 10.0F;
    private Vector3 direction;
    public Vector3 Direction { set { direction = value; } }

    public GameObject parent;
    public GameObject Parent { set { parent = value; } get { return parent; } }

    private SpriteRenderer sprite;
    public Color Color
    {
        set { sprite.color = value; }
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, 3.0F);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();
        Debug.Log(unit);
        if (unit && unit.gameObject != parent)
        {
            Destroy(gameObject);
        }
    }
}
