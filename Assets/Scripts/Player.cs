using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public float height = 1.0f;

    float horizontalMovement = 0.0f;
    float verticalMovement = 0.0f;

    private bool enableKey = false;
    public GameObject objectToSpawn;
    public Vector3 mushroomOffset;

    private MushroomRoot rootMushroom;
    private MushroomNode currentlyIn = null;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        pos.y = pos.y + height;
        transform.position = pos;
        rootMushroom = GameObject.FindWithTag("MushroomRoot").GetComponent<MushroomRoot>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    AnimationState animState = AnimationState.IDLE_R;

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        SpawnMushroom();

        if(horizontalMovement == 0f && verticalMovement == 0f)
        {
            if(animState == AnimationState.RUN_R)
            {
                animState = AnimationState.IDLE_R;
                animator.SetTrigger("IdleRight");
            }
            else if(animState == AnimationState.RUN_L)
            {
                animState = AnimationState.IDLE_L;
                animator.SetTrigger("IdleLeft");
            }
        }
        else if(animState == AnimationState.IDLE_L || animState == AnimationState.IDLE_R)
        {
            if(horizontalMovement < 0f)
            {
                animState = AnimationState.RUN_R;
                animator.SetTrigger("RunRight");
            }
            else if(horizontalMovement > 0f)
            {
                animState = AnimationState.RUN_L;
                animator.SetTrigger("RunLeft");
            }
            else if(horizontalMovement == 0f)
            {
                if(animState == AnimationState.IDLE_R)
                {
                    animState = AnimationState.RUN_R;
                    animator.SetTrigger("RunRight");
                }
                else
                {
                    animState = AnimationState.RUN_L;
                    animator.SetTrigger("RunLeft");
                }
            }
        }
        else
        {
            if(horizontalMovement < 0f && animState != AnimationState.RUN_R)
            {
                animState = AnimationState.RUN_R;
                animator.SetTrigger("RunRight");
            }
            else if(horizontalMovement > 0f && animState != AnimationState.RUN_L)
            {
                animState = AnimationState.RUN_L;
                animator.SetTrigger("RunLeft");
            }
        }
    }

    void FixedUpdate()
    {
        VerticalMovementPlayer();
        HorizontalMovementPlayer();
    }

    void VerticalMovementPlayer()
    {
        // if (verticalMovement < 0.0f)
        // {
        //     Vector3 pos = transform.position;
        //     pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        //     pos.y = pos.y + height;
        //     pos.z = pos.z + movementSpeed;
        //     Raycast(pos);
        //     transform.position = pos;
        // } else if (verticalMovement > 0.0f)
        // {
        //     Vector3 pos = transform.position;
        //     pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        //     pos.y = pos.y + height;
        //     pos.z = pos.z - movementSpeed;
        //     Raycast(pos);
        //     transform.position = pos;
        // }
        rb.velocity = new Vector3(rb.velocity.x, 0, movementSpeed);
    }

    void HorizontalMovementPlayer()
    {
        // if (horizontalMovement < 0.0f)
        // {
        //     Vector3 pos = transform.position;
        //     pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        //     pos.y = pos.y + height;
        //     pos.x = pos.x + movementSpeed;
        //     Raycast(pos);
        //     transform.position = pos;
        // }
        // else if (horizontalMovement > 0.0f)
        // {
        //     Vector3 pos = transform.position;
        //     pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        //     pos.y = pos.y + height;
        //     pos.x = pos.x - movementSpeed;
        //     Raycast(pos);
        //     transform.position = pos;
        // }
        rb.velocity = new Vector3(movementSpeed, 0, rb.velocity.z);
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position) + height;
        transform.position = pos;
    }

    private bool Raycast(Vector3 endPosition)
    {
        Vector3 direction = (endPosition - transform.position);
        
        bool hit = Physics.Raycast(transform.position, direction, direction.magnitude * 1.01f, 0, QueryTriggerInteraction.Ignore);
        if(hit)
        {
            Debug.DrawRay(transform.position, direction * 1.01f, Color.red, 0.125f);
        }
        else
        {
            Debug.DrawRay(transform.position, direction * 1.01f, Color.blue, 0.125f);
        }
        return hit;
    }

    void SpawnMushroom()
    {
        if (enableKey && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed e");
            GameObject newMushroom = Instantiate(objectToSpawn);
            MushroomNode component = newMushroom.GetComponent<MushroomNode>();

            Vector3 pos = transform.position + mushroomOffset;

            newMushroom.transform.position = pos;

            rootMushroom.children.Add(component);
            currentlyIn.children.Add(component);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Mushroom"))
        {
            currentlyIn = collider.GetComponent<MushroomNode>();
            if(currentlyIn.Grown())
            {
                enableKey = true;
                Debug.Log("I have entered");
            }
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Mushroom"))
        {
            enableKey = false;
            Debug.Log("I have exited");
            currentlyIn = null;
        }
    }
}

public enum AnimationState
{
    IDLE_R, IDLE_L, RUN_R, RUN_L
}