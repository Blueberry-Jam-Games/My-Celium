using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTransitionManager : MonoBehaviour
{
    public int UNCORRUPTED = 0;
    public int UNCORRUPTED_MUSHROOM = 1;
    public int CORRUPTED = 2;
    public int CORRUPTED_MUSHROOM = 3;

    public int bottomX;
    public int topX;
    public int bottomY;
    public int topY;

    private Terrain terrain;
    private TerrainData tData;

    private float[,,] map;
    private float[,,] lineBuffer; // cache of the line buffer for speed

    private int width;
    private int height;

    void Start()
    {
        terrain = Terrain.activeTerrain;
        tData = terrain.terrainData;

        width = tData.alphamapWidth;
        height = tData.alphamapHeight;

        map = new float[width, height, tData.alphamapLayers];
        lineBuffer = new float[width, height, 1];

        if(bottomX < 0) bottomX = 0;
        if(bottomX > width) bottomX = width;
        
        if(topX < bottomX) topX = bottomX;
        if(topX > width) topX = width;

        if(bottomY < 0) bottomY = 0;
        if(bottomY > height) bottomY = height;

        if(topY < bottomY) topY = bottomY;
        if(topY > height) topY = height;

        ClearBuffers();
        UpdateMap();
    }

    void Update()
    {
        clearBuffersRestricted();

        List<MushroomStub> mushrooms = null; // TODO get from somewhere
        Line line = new Line(0f, 0f, 0f, 0f);

        // 1. Draw lines at target rate
        for(int i = 0, count = mushrooms.Count; i < count; i++)
        {
            MushroomStub current = mushrooms[i];
            for(int child = 0, children = current.children.Count; i < children; i++)
            {
                MushroomStub currentChild = current.children[child];
                line.Apply(current.position, currentChild.position);
                line.DrawPartial(lineBuffer, 0, currentChild.growth);
            }
        }
        // 2. Circle
        //    Get the farthest and second farthest mushroom
        int centerX = 0, centerY = 0; // The int coordinates of the root mushroom
        float farthestDistance = 0f;
        // float secondFarthestDistance = 0f;

        int roundedRadius = Mathf.RoundToInt(farthestDistance);

        float farthestSquared = 0f;
        float secondSquared = 0f;

        int startY = (centerY - roundedRadius);
        int endY = (centerY + roundedRadius);

        for(int x = (centerX - roundedRadius), xc = (centerX + roundedRadius); x < xc; x++)
        {
            for(int y = startY, yc = endY; y < yc; y++)
            {
                if(x < 0 || x >= width || y < 0 || y >= height)
                {
                    // pass
                }
                else
                {
                    float corruptionAmount = 0f;
                    float squareDistance = x * x + y * y;
                    if (squareDistance < secondSquared)
                    {
                        corruptionAmount = 1f;
                    }
                    else if (squareDistance > secondSquared && squareDistance < farthestSquared)
                    {
                        corruptionAmount = (squareDistance - secondSquared) / (farthestSquared - secondSquared);
                    }

                    float mushroomAmount = lineBuffer[x, y, 0];

                    map[x, y, CORRUPTED_MUSHROOM] = corruptionAmount * mushroomAmount;
                    map[x, y, UNCORRUPTED_MUSHROOM] = (1 - corruptionAmount) * mushroomAmount;
                    map[x, y, CORRUPTED] = corruptionAmount * (1 - mushroomAmount);
                    map[x, y, UNCORRUPTED] = (1 - corruptionAmount) * (1 - mushroomAmount);
                }
            }
        }

        UpdateMap();
    }

    private void ClearBuffers()
    {
        int maxY = map.GetLength(1);
        for(int x = 0, xc = map.GetLength(0); x < xc; x++)
        {
            for(int y = 0, yc = maxY; y < yc; y++)
            {
                lineBuffer[x, y, 0] = 0f;
                for(int z = 0, zc = map.GetLength(2); z < zc; z++)
                {
                    map[x, y, z] = (z == 0 ? 1f : 0f);
                }
            }
        }
    }

    private void clearBuffersRestricted()
    {
        for(int x = bottomX, xc = topX; x < xc; x++)
        {
            for(int y = bottomY, yc = topY; y < yc; y++)
            {
                lineBuffer[x, y, 0] = 0f;
                for(int z = 0, zc = map.GetLength(2); z < zc; z++)
                {
                    map[x, y, z] = (z == 0 ? 1f : 0f);
                }
            }
        }
    }

    private void UpdateMap()
    {
        tData.SetAlphamaps(0, 0, map);
    }
}

public struct MushroomStub
{
    public Vector3 position;
    public float growth;
    public List<MushroomStub> children;
}