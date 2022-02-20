using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputs input;
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] private Animator anim;

    private Rigidbody2D rb;
    public Transform movePoint;
    public Vector3 oldMovePoint;

    private float movespeed = 4f;

    public LayerMask stopMovementLayer;
    private int moveIssueCounter;

    private static string doorDirection;

    public bool move = false;
    public int movingCount;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void Start()
    {
        DontDestroyOnLoadManager.DontDestroyOnLoad(movePoint.gameObject);
        movePoint.parent = null;
    }

    void Update()
    {
        Animate();
        Movement();
    }

    private void FixedUpdate()
    {
        // the moveIssueCounter is in order to stop the player getting stuck inside of objects by reseting the movement position after this counter gets too high.
        moveIssueCounter++;
        if (move == false) movingCount++;
        else movingCount = 0;
    }

    public void StopMove()
    {
        movePoint.position = oldMovePoint;
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movespeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) <= .001f)
        {

            moveIssueCounter = 0;
            oldMovePoint = transform.position;
            if (Mathf.Abs(input.horizontal) == 1f && input.vertical == 0)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(input.horizontal, 0f, 0f), .48f, stopMovementLayer))
                {
                    movePoint.position += new Vector3(input.horizontal, 0f, 0f);
                    anim.SetBool("moving", true);
                    input.vertical = 0;
                    input.horizontal = 0;
                }

            }
            else if (Mathf.Abs(input.vertical) == 1f && input.horizontal == 0)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, input.vertical, 0f), .48f, stopMovementLayer))
                {
                    movePoint.position += new Vector3(0f, input.vertical, 0f);
                    anim.SetBool("moving", true);
                    input.vertical = 0;
                    input.horizontal = 0;
                }
            }
            else { move = false; anim.SetBool("moving", false); }
        }
        else if (moveIssueCounter >= 50)  { StopMove(); anim.SetBool("moving", false); }
        else move = true; 
    }
    
    private void Animate()
    {
        if (input.horizontal == 1f)
        {
            spriteRend.flipX = true;
        } else if (input.horizontal == -1f)
        {
            spriteRend.flipX = false;
        }
    }
    
    public Vector3 DoorPosition()
    {
        if (doorDirection == "N") { return new Vector3(0.5f, -3.5f, 0); }
        else if (doorDirection == "E") { return new Vector3(-8.5f, 0.5f, 0); }
        else if (doorDirection == "S") { return new Vector3(0.5f, 4.5f, 0); }
        else if (doorDirection == "W") { return new Vector3(9.5f, 0.5f, 0); }
        else return new Vector3(0.5f, 0.5f, 0);
    }

    public void SetDoorDirection(string inDir)
    {
        doorDirection = inDir;
    }
}
