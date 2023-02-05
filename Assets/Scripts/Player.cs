using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class Player : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public float height = 1.0f;

    float horizontalMovement = 0.0f;
    float verticalMovement = 0.0f;

    private bool enableKey = false;
    public GameObject objectToSpawn;
    public Vector3 mushroomOffset;

    public VisualEffect dustRight;
    public VisualEffect dustLeft;

    private MushroomRoot rootMushroom;
    private MushroomNode currentlyIn = null;

    private Rigidbody rb;
    private Animator animator;
    private GameplayManager gameplayManager;

    [Header("Spore Increases")]
    public int spore1Increase = 10;
    public int spore2Increase = 10;
    public int spore3Increase = 10;

    private PopUpScreens popUp;

    private AnimationState animState = AnimationState.IDLE_R;


    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        pos.y = pos.y + height;
        transform.position = pos;
        rootMushroom = GameObject.FindWithTag("MushroomRoot").GetComponent<MushroomRoot>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameplayManager = GameObject.FindWithTag("GameplayManager").GetComponent<GameplayManager>();
        popUp = GameObject.FindWithTag("PopUps").GetComponent<PopUpScreens>();
    }

    private IEnumerator AttackFunction()
    {
        yield return new WaitForSeconds(1.05f);
        if(animState == AnimationState.ATK_R)
        {
            dustRight.Play();
        }
        else if(animState == AnimationState.ATK_L)
        {
            dustLeft.Play();
        }

        yield return new WaitForSeconds(0.15f);

        if(animState == AnimationState.ATK_R)
        {
            animState = AnimationState.IDLE_R;
        }
        else if(animState == AnimationState.ATK_L)
        {
            animState = AnimationState.ATK_L;
        }
    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space) && (animState != AnimationState.ATK_R || animState != AnimationState.ATK_L))
        {
            if(animState == AnimationState.RUN_R || animState == AnimationState.IDLE_R)
            {
                animState = AnimationState.ATK_R;
                animator.SetTrigger("AtkRight");
            }
            else
            {
                animState = AnimationState.ATK_L;
                animator.SetTrigger("AtkLeft");
            }
            rb.velocity = Vector3.zero;
            StartCoroutine(AttackFunction());
        }

        if(animState != AnimationState.ATK_R || animState != AnimationState.ATK_L)
        {
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
    }

    void FixedUpdate()
    {
        VerticalMovementPlayer();
        HorizontalMovementPlayer();
    }

    void VerticalMovementPlayer()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, verticalMovement * movementSpeed * -1f);
    }

    void HorizontalMovementPlayer()
    {
        rb.velocity = new Vector3(horizontalMovement * movementSpeed * -1f, 0f, rb.velocity.z);
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position) + height;
        transform.position = pos;
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
            popUp.EnableTextPopUpUpgrade("Press e to plant a Mushroom");
            Debug.Log("I have entered");
            currentlyIn = collider.GetComponent<MushroomNode>();
            if(currentlyIn.Grown())
            {
                enableKey = true;
                Debug.Log("I have entered");
            }
        } else if (collider.CompareTag("Cauldron"))
        {
            popUp.EnableCauldronUpgrade();
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Mushroom"))
        {
            popUp.DisableTextPopUpUpgrade();
            enableKey = false;
            Debug.Log("I have exited");
            currentlyIn = null;
        }
    }
}

public enum AnimationState
{
    IDLE_R, IDLE_L, RUN_R, RUN_L, ATK_R, ATK_L
}