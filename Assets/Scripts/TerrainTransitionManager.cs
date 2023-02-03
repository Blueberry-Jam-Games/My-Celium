using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTransitionManager : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData tData;

    private float[,,] map;

    void Start()
    {
        terrain = Terrain.activeTerrain;
        tData = terrain.terrainData;

        map = new float[tData.alphamapWidth, tData.alphamapHeight, tData.alphamapLayers];

        for(int x = 0, xc = map.GetLength(0); x < xc; x++)
        {
            for(int y = 0, yc = map.GetLength(1); y < yc; y++)
            {
                for(int z = 0, zc = map.GetLength(2); z < zc; z++)
                {
                    map[x, y, z] = (z == 0 ? 1f : 0f);
                }
            }
        }

        tData.SetAlphamaps(0, 0, map);
    }

    void Update()
    {
        // 1. Draw lines at target rate
        // 2. Circle
        //    Get the farthest and second farthest mushroom
        //    radius = distance to farthest mushroom
        //    for loop on radius
        //        distance < second farthest -> 100% corrupt
        //        distance > second farthest < farthest -> lerp% corrupt
        //        distance > farthest -> 0% corrupt
    }
}
