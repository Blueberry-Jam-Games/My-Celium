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

    private GameplayManager gameplayManager;

    [Header("Spore Increases")]
    public int spore1Increase = 10;
    public int spore2Increase = 10;
    public int spore3Increase = 10;

    private PopUpScreens popUp;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        pos.y = pos.y + height;
        transform.position = pos;
        rootMushroom = GameObject.FindWithTag("MushroomRoot").GetComponent<MushroomRoot>();
        gameplayManager = GameObject.FindWithTag("GameplayManager").GetComponent<GameplayManager>();
        popUp = GameObject.FindWithTag("PopUps").GetComponent<PopUpScreens>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        VerticalMovementPlayer();
        HorizontalMovementPlayer();
        SpawnMushroom();
    }

    void VerticalMovementPlayer()
    {
        if (verticalMovement < 0.0f)
        {
            Vector3 pos = transform.position;
            pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            pos.y = pos.y + height;
            pos.z = pos.z + movementSpeed;
            transform.position = pos;
        } else if (verticalMovement > 0.0f)
        {
            Vector3 pos = transform.position;
            pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            pos.y = pos.y + height;
            pos.z = pos.z - movementSpeed;
            transform.position = pos;
        }
    }

    void HorizontalMovementPlayer()
    {
        if (horizontalMovement < 0.0f)
        {
            Vector3 pos = transform.position;
            pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            pos.y = pos.y + height;
            pos.x = pos.x + movementSpeed;
            transform.position = pos;
        } else if (horizontalMovement > 0.0f)
        {
            Vector3 pos = transform.position;
            pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            pos.y = pos.y + height;
            pos.x = pos.x - movementSpeed;
            transform.position = pos;
        }
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

            if (gameplayManager.state == GameplayManager.gameState.Level1)
            {
                gameplayManager.SetSpore1(gameplayManager.GetSpore1() + spore1Increase);
            } else if (gameplayManager.state == GameplayManager.gameState.Level2)
            {
                gameplayManager.SetSpore1(gameplayManager.GetSpore1() + spore1Increase);
                gameplayManager.SetSpore2(gameplayManager.GetSpore2() + spore2Increase);
            } else {
                gameplayManager.SetSpore1(gameplayManager.GetSpore1() + spore1Increase);
                gameplayManager.SetSpore2(gameplayManager.GetSpore2() + spore2Increase);
                gameplayManager.SetSpore3(gameplayManager.GetSpore3() + spore3Increase);
            }

            popUp.UpdateSporeCounter();
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
