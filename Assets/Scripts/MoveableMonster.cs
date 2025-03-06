using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveableMonster : Monster
{
    [SerializeField]
    private float speed = 2.0F;

    [SerializeField]
    private AudioSource deadSound;

    private Vector3 direction;

    private SpriteRenderer sprite;
    protected override void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        direction = transform.right;
    }

    protected override void Update()
    {
        Move();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 0.5F)
            {
                deadSound.Play();
                Invoke("ReceiveDamage", 0.2F);
            }
            else unit.ReceiveDamage();
        }
    }

    private void Move()
    {
        Collider2D[] collidersNear = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + transform.right * direction.x * 0.5F, 0.1F);
        Collider2D[] collidersFar = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + transform.right * direction.x * 4.0F, 1F);

        var characterDontNearby = collidersNear.All(x => !x.GetComponent<Character>());
        var characterNearby = collidersFar.Any(x => x.GetComponent<Character>());
        if (collidersNear.Length > 1 && characterDontNearby)
        {
            direction *= -1.0F;
            speed = 2.0F;
        }
        if (characterNearby) speed = 3.0F;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
