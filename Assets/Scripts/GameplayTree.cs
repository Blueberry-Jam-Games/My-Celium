using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayTree : MonoBehaviour
{
    public GameObject normal;
    public GameObject corrupt;
    public GameObject log;

    private int captureCount = 0;

    public int state = 0; // 0 = normal, 1 = corrupted, 2 = log
    public float captureTime = 10f;
    public float logTime = 10f;

    void Start()
    {
        UpdateState();
    }

    public void AddCapture()
    {
        captureCount++;
        if (captureCount == 1)
        {
            StartCoroutine(Capture());
        }
    }

    public void Uncapture()
    {
        captureCount--;
    }

    private void UpdateState()
    {
        normal.SetActive(state == 0);
        corrupt.SetActive(state == 1);
        // log.SetActive(state == 2);
    }

    private IEnumerator Capture()
    {
        yield return new WaitForSeconds(captureTime);
        state = 1;
        UpdateState();

        if(captureCount != 0)
        {
            yield return new WaitForSeconds(logTime);
            state = 2;
            UpdateState();
            yield return Collapse();
        }
    }

    private IEnumerator Collapse()
    {
        yield return null;
        // rotate log down
        // play particle effect
    }
}
