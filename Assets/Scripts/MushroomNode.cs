using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomNode : MushroomHolder
{
    public float height = 0;
    public float growthTime = 15;

    private float startTime;
    public float growth = 0f;

    public float Growth {get => growth;}

    // Start is called before the first frame update
    void Start()
    {
        children = new List<MushroomNode>();

        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        transform.position = pos;

        startTime = Time.time;

        UpdateScale();
    }

    // Update is called once per frame
    void Update()
    {
        if(growth < 1f)
        {
            float deltaTime = Time.time - startTime;
            growth = deltaTime / growthTime;
            if(growth >= 1f)
            {
                growth = 1f;
            }
            UpdateScale();
        }
        else
        {
            // Can produce spores and expand
        }
    }

    public bool Grown()
    {
        return growth >= 1f;
    }

    private void UpdateScale()
    {
        float newScale = Mathf.Pow(Mathf.Lerp(0.25f, 1f, growth), 2);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
