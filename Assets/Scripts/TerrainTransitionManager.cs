using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTransitionManager : MonoBehaviour
{
    public int UNCORRUPTED = 0;
    public int UNCORRUPTED_MUSHROOM = 1;
    public int CORRUPTED = 2;
    public int CORRUPTED_MUSHROOM = 3;

    // public int bottomX;
    // public int topX;
    // public int bottomY;
    // public int topY;

    private Terrain terrain;
    private TerrainData tData;

    private float[,,] map;
    private float[,,] lineBuffer; // cache of the line buffer for speed

    private int width;
    private int height;

    private GameObject rootMushroom;

    void Start()
    {
        terrain = Terrain.activeTerrain;
        tData = terrain.terrainData;

        width = tData.alphamapWidth;
        height = tData.alphamapHeight;

        map = new float[width, height, tData.alphamapLayers];
        lineBuffer = new float[width, height, 1];

        Debug.Log("Terrain transition manager initialized at " + width + ", " + height + " layers " + tData.alphamapLayers);

        // if(topX == -1) topX = width;
        // if(topY == -1) topY = height;

        // if(bottomX < 0) bottomX = 0;
        // if(bottomX > width) bottomX = width;
        
        // if(topX < bottomX) topX = bottomX;
        // if(topX > width) topX = width;

        // if(bottomY < 0) bottomY = 0;
        // if(bottomY > height) bottomY = height;

        // if(topY < bottomY) topY = bottomY;
        // if(topY > height) topY = height;

        ClearBuffers();
        UpdateMap();
        rootMushroom = GameObject.FindGameObjectWithTag("MushroomRoot");
    }

    void Update()
    {
        ClearBuffers();
        GameObject[] mushroomObjs = GameObject.FindGameObjectsWithTag("Mushroom");
        List<MushroomNode> mushrooms = new List<MushroomNode>(mushroomObjs.Length); // TODO get from somewhere
        for(int i = 0, count = mushroomObjs.Length; i < count; i++)
        {
            mushrooms.Add(mushroomObjs[i].GetComponent<MushroomNode>());
        }
        Line line = new Line(0f, 0f, 0f, 0f);

        // 1. Draw lines at target rate
        for(int i = 0, count = mushrooms.Count; i < count; i++)
        {
            MushroomNode current = mushrooms[i];
            for(int child = 0, children = current.children.Count; child < children; child++)
            {
                MushroomNode currentChild = current.children[child];

                // Debug.Log($"Generating line between {current.name} at {current.transform.position} to {currentChild.name} at {currentChild.transform.position}");

                Debug.DrawLine(current.transform.position, currentChild.transform.position, Color.red, 0.25f);

                line.Apply(current.transform.position + new Vector3(1, 0, 2), currentChild.transform.position + new Vector3(1, 0, 2));
                line.DrawPartial(lineBuffer, 0, currentChild.Growth);
            }
        }
        // 2. Circle
        //    Get the farthest and second farthest mushroom
        int centerX = Mathf.RoundToInt(rootMushroom.transform.position.x);
        int centerY = Mathf.RoundToInt(rootMushroom.transform.position.z); // The int coordinates of the root mushroom
        Vector3 rootTransform = rootMushroom.transform.position;

        mushrooms.Sort((m1, m2) => {
            return (m1.transform.position - rootTransform).sqrMagnitude > (m2.transform.position - rootTransform).sqrMagnitude ? -1 : 1;
        });

        float farthestDistance = Vector3.Distance(rootTransform, mushrooms[0].transform.position);
        // float secondFarthestDistance = 0f;

        int roundedRadius = Mathf.RoundToInt(farthestDistance) + 1;

        float farthestSquared = farthestDistance * farthestDistance;
        float secondSquared = 25f;
        
        if(mushrooms.Count > 1)
        {
            secondSquared = (rootTransform - mushrooms[1].transform.position).sqrMagnitude;
        }

        centerX += 1;
        centerY += 2;
        int startY = (centerY - roundedRadius);
        int endY = (centerY + roundedRadius);

        // Debug.Log($"Drawing circle at {roundedRadius}, and coords {centerX},{centerY} + with inner radius {secondSquared}");

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
                    float squareDistance = (Mathf.Pow(x - centerX, 2) + Mathf.Pow(y - centerY, 2));
                    if (squareDistance < secondSquared)
                    {
                        corruptionAmount = 1f;
                    }
                    else if (squareDistance > secondSquared && squareDistance < farthestSquared)
                    {
                        corruptionAmount = (squareDistance - secondSquared) / (farthestSquared - secondSquared);
                    }

                    float mushroomAmount = lineBuffer[x, y, 0];

                    // Debug.Log($"Writing slot {x},{y}");

                    // idk why but x and z are swapped (y becomes z)
                    // if(corruptionAmount != 0f)
                    // {
                    //     Debug.Log($"Position {x},{y} has corruption {corruptionAmount}");
                    // }

                    map[y, x, CORRUPTED_MUSHROOM] = corruptionAmount * mushroomAmount;
                    map[y, x, UNCORRUPTED_MUSHROOM] = (1 - corruptionAmount) * mushroomAmount;
                    map[y, x, CORRUPTED] = corruptionAmount * (1 - mushroomAmount);
                    map[y, x, UNCORRUPTED] = (1 - corruptionAmount) * (1 - mushroomAmount);
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
                    map[x, y, z] = (z == UNCORRUPTED ? 1f : 0f);
                }
            }
        }
    }

    // private void clearBuffersRestricted()
    // {
    //     for(int x = bottomX, xc = topX; x < xc; x++)
    //     {
    //         for(int y = bottomY, yc = topY; y < yc; y++)
    //         {
    //             lineBuffer[x, y, 0] = 0f;
    //             for(int z = 0, zc = map.GetLength(2); z < zc; z++)
    //             {
    //                 map[x, y, z] = (z == 0 ? 1f : 0f);
    //             }
    //         }
    //     }
    // }

    private void UpdateMap()
    {
        tData.SetAlphamaps(0, 0, map);
        terrain.Flush();
        // Terrain.activeTerrain.terrainData.SetAlphamaps(0, 0, map);
    }
}

// public struct MushroomStub
// {
//     public Vector3 position;
//     public float growth;
//     public List<MushroomStub> children;
// }