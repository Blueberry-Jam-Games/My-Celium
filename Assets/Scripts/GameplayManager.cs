using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public int spore1 = 0;
    public int spore2 = 0;
    public int spore3 = 0;

    public gameState state;
    // Start is called before the first frame update
    void Start()
    {
        state = gameState.Level1;
        spore1 = 0;
        spore2 = 0;
        spore3 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public enum gameState
    {
        Level1,
        Level2,
        Level3
    }
}

