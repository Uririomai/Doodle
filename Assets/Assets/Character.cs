using UnityEngine;
using UnityEngine.SceneManagement;


public class Character : Unit  
{
    [SerializeField]
    private int lives = 5;

    public int Lives
    {
        get { return lives; }
        set
        {
            if (value <= 5) lives = value;
            livesBar.Refresh();
        }
    }
    private LivesBar livesBar;

    [SerializeField]
    private AudioSource jumpSound;

    [SerializeField]
    private AudioSource shootSound;

    [SerializeField]
    private AudioSource damageSound;

    [SerializeField]
    private AudioSource deadSound;

    [SerializeField]
    private float speed = 3.0F;

    [SerializeField]
    private float jumpForce = 15.0F;

    [SerializeField]
    private LayerMask platformLayerMask;

    private Bullet bullet;


    new private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider2d;
    private Animator animator;
    private SpriteRenderer sprite;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        livesBar = FindObjectOfType<LivesBar>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider2d = GetComponent<BoxCollider2D>();

        bullet = Resources.Load<Bullet>("Bullet");
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (CheckGround()) State = CharState.Idle;

        if (Input.GetButton("Horizontal")) Run();

        if (CheckGround() && Input.GetButtonDown("Jump")) Jump();

        if (Input.GetButtonDown("Fire1")) Shoot();

        if (transform.position.y < -30.0F) Lives--;

        if (Lives == 0)
        {
            deadSound.Play();
            Invoke("RestartLevel", 1F);
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Run()
    {

        State = CharState.Walk;
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;
    }

    private void Jump()
    {
        State = CharState.Jump;
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        jumpSound.Play();
    }

    private void Shoot()
    {
        Vector3 position = transform.position;
        position.x += 0.6F * (sprite.flipX ? -1.0F : 1.0F);
        position.y -= 0.1F;

        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation);

        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F);
        shootSound.Play();
    }

    public override void ReceiveDamage()
    {
        Lives--;
        
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 10F, ForceMode2D.Impulse);
        Debug.Log(lives);
        damageSound.Play();
    }

    private bool CheckGround()
    {
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.7F);

        // isGrounded = colliders.Length > 1;
        float extraHeightText = 0.05F;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);
        Color rayColor;
        if (raycastHit.collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;

        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x * 2f), rayColor);

        return raycastHit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject)
        {
            ReceiveDamage();
        }
    }

}

public enum CharState
{
    Idle,
    Walk,
    Jump
}