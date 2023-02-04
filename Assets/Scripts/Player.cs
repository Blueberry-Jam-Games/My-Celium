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
    public float spacing = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        pos.y = pos.y + height;
        transform.position = pos;
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
        if (enableKey && Input.GetKeyDown("e"))
        {
            Debug.Log("Pressed e");
            GameObject newMushroom = Instantiate(objectToSpawn);

            Vector3 pos = transform.position;
            pos.x = pos.x - spacing;
            
            newMushroom.transform.position = pos;

            GameObject rootMushroom = GameObject.FindWithTag("MushroomRoot");

            rootMushroom.GetComponent<MushroomRoot>().mushrooms.Add(newMushroom);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Mushroom"))
        {
            enableKey = true;
            Debug.Log("I have entered");
        } else if (collider.CompareTag("Cauldron"))
        {
            GameObject popUp = GameObject.FindWithTag("PopUps");
            popUp.GetComponent<PopUpScreens>().EnableCauldronUpgrade();
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Mushroom"))
        {
            enableKey = false;
            Debug.Log("I have exited");
        }
    }
}
