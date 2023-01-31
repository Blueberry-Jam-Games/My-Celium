using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTerrain : MonoBehaviour
{
    private void Start()
    {
        Terrain t = Terrain.activeTerrain;
        float[,,] map = new float[10, 10, 2];

        // For each point on the alphamap...
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                // Get the normalized terrain coordinate that
                // corresponds to the point.
                float normX = x * 1.0f / (10 - 1);
                float normY = y * 1.0f / (10 - 1);

                // Get the steepness value at the normalized coordinate.
                var angle = t.terrainData.GetSteepness(normX, normY);

                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                var frac = angle / 90.0;
                map[x, y, 1] = (float)frac;
                map[x, y, 0] = (float)(1 - frac);
            }
        }
        t.terrainData.SetAlphamaps(25, 25, map);
    }

    private void Update()
    {

    }
}
