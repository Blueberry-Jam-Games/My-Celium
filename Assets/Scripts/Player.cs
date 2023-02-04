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


    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        pos.y = pos.y + height;
        transform.position = pos;
        rootMushroom = GameObject.FindWithTag("MushroomRoot").GetComponent<MushroomRoot>();
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
