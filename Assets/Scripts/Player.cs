using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = -2.0f;
    public float height = 6.0f;

    float horizontalMovement = 0.0f;
    float verticalMovement = 0.0f;
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
    }

    void VerticalMovementPlayer()
    {
        if (verticalMovement > 0.0f)
        {
            Vector3 pos = transform.position;
            pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            pos.y = pos.y + height;
            pos.z = pos.z + movementSpeed;
            transform.position = pos;
        } else if (verticalMovement < 0.0f)
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
        if (horizontalMovement > 0.0f)
        {
            Vector3 pos = transform.position;
            pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            pos.y = pos.y + height;
            pos.x = pos.x + movementSpeed;
            transform.position = pos;
        } else if (horizontalMovement < 0.0f)
        {
            Vector3 pos = transform.position;
            pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
            pos.y = pos.y + height;
            pos.x = pos.x - movementSpeed;
            transform.position = pos;
        }
    }
}
