using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomNode : MushroomHolder
{
    public float height = 0;
    public float[] growthTime = new float[]{15, 10, 20};

    private float startTime;
    public float growth = 0f;

    public float Growth {get => growth;}

    public int[] VariantSpores = new int[]{5, 10, 15};

    private bool initialGrowth = false;

    private GameplayManager gameplayManager;

    void Start()
    {
        gameplayManager = GameObject.FindWithTag("GameplayManager").GetComponent<GameplayManager>();

        children = new List<MushroomNode>();

        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        transform.position = pos;

        startTime = Time.time;

        UpdateScale();
    }

    void Update()
    {
        if(growth < 1f)
        {
            float deltaTime = Time.time - startTime;
            growth = deltaTime / growthTime[0]; // TODO replace [0] with variant 
            if(growth >= 1f)
            {
                growth = 1f;
            }
            UpdateScale();
        }
        else
        {
            if(!initialGrowth)
            {
                initialGrowth = true;
                StartCoroutine(ProduceSpores());
                // Some visual effect about being grown
            }
        }
    }

    private IEnumerator ProduceSpores()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);
        while(this.isActiveAndEnabled)
        {
            gameplayManager.AddSpores(0, VariantSpores[0]); // TODO Replace 0 with the spore variant
            yield return delay;

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
