using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMonster : Monster
{
    [SerializeField]
    private float rate = 2.0F;

    [SerializeField]
    private AudioSource shootSound;

    [SerializeField]
    private AudioSource deadSound;

    private Bullet bullet;
    private Color bulletColor = Color.red;

    protected override void Awake()
    {
        bullet = Resources.Load<Bullet>("Bullet");
    }

    protected override void Start()
    {
        InvokeRepeating("Shoot", rate, rate);
    }

    private void Shoot()
    {
        Vector3 position = transform.position;

        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation);

        newBullet.Parent = gameObject;
        newBullet.Direction = -newBullet.transform.right;
        newBullet.Color = bulletColor;
        shootSound.Play();
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
}
