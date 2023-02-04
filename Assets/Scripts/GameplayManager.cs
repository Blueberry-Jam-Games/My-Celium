using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private int spore1 = 0;
    private int spore2 = 0;
    private int spore3 = 0;

    public gameState state;
    // Start is called before the first frame update
    void Start()
    {
        state = gameState.Level1;
        spore1 = 0;
        spore2 = 0;
        spore3 = 0;
    }

    public int GetSpore1()
    {
        return spore1;
    }

    public int GetSpore2()
    {
        return spore2;
    }

    public int GetSpore3()
    {
        return spore3;
    }

    public void SetSpore1(int addition)
    {
        spore1 = addition;
    }

    public void SetSpore2(int addition)
    {
        spore2 = addition;
    }

    public void SetSpore3(int addition)
    {
        spore3 = addition;
    }

    public void IncreaseLevel()
    {
        if (state != gameState.Level3)
        {
            state += 1;
        }
    }

    public enum gameState
    {
        Level1,
        Level2,
        Level3
    }
}

