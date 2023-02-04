using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpScreens : MonoBehaviour
{
    public GameObject cauldronUpgrade;
    public GameObject witchUpgrade;

    [Header("Text References")]
    public GameObject textPopUp;
    public GameObject text;
    private TextMeshPro textObject;

    [Header("Spore References")]
    public GameObject sporeCounter;

    void Start()
    {
        textObject = text.GetComponent<TextMeshPro>();
    }

    public void EnableCauldronUpgrade()
    {
        cauldronUpgrade.GetComponent<Canvas>().enabled = true;
    }

    public void DisableCauldronUpgrade()
    {
        cauldronUpgrade.GetComponent<Canvas>().enabled = false;
    }

    public void EnableWitchUpgrade()
    {
        witchUpgrade.GetComponent<Canvas>().enabled = true;
    }

    public void DisableWitchUpgrade()
    {
        witchUpgrade.GetComponent<Canvas>().enabled = false;
    }

    public void EnableTextPopUpUpgrade(string popUp)
    {
        textPopUp.GetComponent<Canvas>().enabled = true;
        textObject.SetText(popUp);
    }

    public void DisableTextPopUpUpgrade()
    {
        textPopUp.GetComponent<Canvas>().enabled = false;
        textObject.SetText("");
    }

    public void EnableSporeCounterUpgrade()
    {
        sporeCounter.GetComponent<Canvas>().enabled = true;
    }

    public void DisableSporeCounterUpUpgrade()
    {
        sporeCounter.GetComponent<Canvas>().enabled = false;
    }
}
