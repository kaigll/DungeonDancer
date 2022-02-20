using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Main : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;
    [SerializeField] private int moveSpeed;

    private Rigidbody2D rb;
    public Transform movePoint;
    public LayerMask stopMovementLayer;
    public LayerMask doorLayer;
    public LayerMask playerLayer;

    public Vector2Int region;

    Metronome metronome;

    Vector3 nextPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        movePoint.parent = null;
        metronome = FindObjectOfType<Metronome>();
    }

    private void Update()
    {
        Movement();
    }

    public int GetHealth() { return currentHealth; }
    public void Damage(int damage) {
        if (currentHealth - damage < 0) { currentHealth = 0; Death(); }
        else if (currentHealth - damage == 0) { currentHealth = 0; Death(); }
        else currentHealth = currentHealth - damage; Movement();
        //Debug.Log("Health: " + currentHealth);
    }


    private void Death()
    {
        GameObject dungeon = GameObject.FindGameObjectWithTag("Dungeon");
        WorldGeneration gen = dungeon.GetComponent<WorldGeneration>();
        gen.CurrentRoom().Depopulate(region);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (collision.GetComponent<PlayerMovement>().move)
            {
                movePoint.position = collision.GetComponent<PlayerMovement>().oldMovePoint;
                Damage(1);
            }
            else
            {
                collision.GetComponent<PlayerHealth>().Damage(1);
            }
        }
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) <= .001f)
        {
            if (metronome.beatEvent)
            {
                if (Physics2D.OverlapCircle(transform.position, 1.4f, playerLayer))
                {
                    nextPosition = FindObjectOfType<PlayerMovement>().oldMovePoint- this.transform.position;
                } else
                {
                    float randNum = Random.value;
                    if (randNum <= 0.25f) nextPosition = Vector3.up;
                    else if (randNum <= 0.5f) nextPosition = Vector3.down;
                    else if (randNum <= 0.75f) nextPosition = Vector3.left;
                    else if (randNum <= 1.00f) nextPosition = Vector3.right;
                }
            }
            else nextPosition = Vector3.zero;
            if (!Physics2D.OverlapCircle((movePoint.position + nextPosition), .48f, stopMovementLayer) && !Physics2D.OverlapCircle((movePoint.position + nextPosition), .48f, doorLayer))
            {
                movePoint.position += nextPosition;
            }
        }
    }
}
