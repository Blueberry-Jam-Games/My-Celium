using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCapture : MonoBehaviour
{
    private List<GameplayTree> capturedTrees = new List<GameplayTree>();

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Tree"))
        {
            GameplayTree otherTree = collider.GetComponent<GameplayTree>();
            otherTree.AddCapture(0); // TODO replace 0 with mushroom variety
        }
    }

    private void OnDestroyed()
    {
        foreach(GameplayTree tree in capturedTrees)
        {
            tree.Uncapture();
        }
    }
}
