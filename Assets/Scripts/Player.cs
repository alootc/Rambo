using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState { Idle, Running, Jumping, Falling, Dead }

public class Player : Character
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float distance = 0.1f;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Corazones[] hearts_UI;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private Weapon weapon;

    private float xInput;
    private PlayerState currentState;

    private Vector2 direction = Vector2.right;

    private AudioSource audioSource;
    public AudioClip clip;

    // Variable para controlar si ya está muerto
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead) return;

        Flip();
        HandleInput();
        ChangeAnimState();
    }

    void FixedUpdate()
    {
        if (isDead) return;
        HandleMovement();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weapon.Shoot();
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Jump();
        }
    }

    private void HandleMovement()
    {
        Vector2 move = new Vector2(xInput * speed, rb.velocity.y);
        rb.velocity = move;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        if (xInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            direction = Vector2.right;
        }
        else if (xInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            direction = Vector2.left;
        }
    }

    private void ChangeAnimState()
    {
        if (isGrounded())
        {
            if (xInput == 0)
            {
                ChangeState(PlayerState.Idle);
            }
            else
            {
                ChangeState(PlayerState.Running);
            }
        }
        else
        {
            if (rb.velocity.y < -0.1f)
            {
                ChangeState(PlayerState.Falling);
            }
            else if (rb.velocity.y > 0.1f)
            {
                ChangeState(PlayerState.Jumping);
            }
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            coll.bounds.center,
            coll.bounds.size,
            0f,
            Vector2.down,
            distance,
            whatIsGround
        );
        return hit.collider != null;
    }

    public void ChangeState(PlayerState newState)
    {
        if (newState == currentState) return;

        currentState = newState;

        switch (newState)
        {
            case PlayerState.Idle:
                anim.SetTrigger("Idle");
                break;
            case PlayerState.Running:
                anim.SetTrigger("Correr");
                break;
            case PlayerState.Jumping:
                anim.SetTrigger("Saltar");
                break;
            case PlayerState.Falling:
                anim.SetTrigger("Caer");
                break;
            case PlayerState.Dead:
                anim.SetTrigger("Muere");
                break;
        }
    }

    public Vector2 GetDirection() { return direction; }

    public override void TakeDamage()
    {
        if (isDead) return;

        // Reducir corazones
        hearts--;
        Debug.Log($"Daño recibido. Corazones restantes: {hearts}");

        // Actualizar UI de corazones
        if (hearts_UI != null && hearts >= 0 && hearts < hearts_UI.Length)
        {
            hearts_UI[hearts].SetHeartEmpty();
        }

        if (hearts <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        ChangeState(PlayerState.Dead);

        // Desactivar colisiones y física
        if (coll != null) coll.enabled = false;
        if (rb != null) rb.velocity = Vector2.zero;

        Debug.Log("Jugador murió");
        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Perdiste");
    }

    public bool IsDead()
    {
        return isDead;
    }
}