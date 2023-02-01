using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenu : MonoBehaviour
{

    public List<MenuButton> buttons = new List<MenuButton>();
    private Vector2 Mouseposition;
    private Vector2 fromVector2M = new Vector2(0.5f, 1.0f);
    private Vector2 centerCircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVector2M;

    public Transform canvas;

    public int menuItems;
    public int currentMenuItem;
    private int oldMenuItem;

    // Start is called before the first frame update
    void Start()
    {
        
        menuItems = buttons.Count;
        foreach(MenuButton button in buttons){
            button.sceneImage.color = button.NormalColor;
        }

        currentMenuItem = 0;
        oldMenuItem = 0;

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Tab)){

            Pause();

        }
        
        if(canvas.gameObject.activeInHierarchy == true){

            getCurrentMenuItem();
            if(Input.GetMouseButtonDown(0)){
                ButtonAction();
            }
            else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)){
                keyAction();
            }

        }

    }

    public void getCurrentMenuItem(){

        Mouseposition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

        toVector2M = new Vector2 (Mouseposition.x / Screen.width, Mouseposition.y / Screen.height);

        float angle = (Mathf.Atan2(fromVector2M.y-centerCircle.y, fromVector2M.x - centerCircle.x) - Mathf.Atan2(toVector2M.y-centerCircle.y, toVector2M.x - centerCircle.x)) * Mathf.Rad2Deg;

        if(angle < 0){
            angle += 360;
        }

        currentMenuItem = (int) (angle / (360 / menuItems));

        if(currentMenuItem != oldMenuItem){

            buttons[oldMenuItem].sceneImage.color = buttons[oldMenuItem].NormalColor;
            oldMenuItem = currentMenuItem;
            buttons[currentMenuItem].sceneImage.color = buttons[currentMenuItem].HighlightedColor;

        }

    }

    public void ButtonAction(){

        buttons[currentMenuItem].sceneImage.color = buttons[currentMenuItem].PressedColor;
        if(currentMenuItem == 0){

            print("Option 1");

        }
        else if(currentMenuItem == 1){

            print("Option 2");

        }
        else if(currentMenuItem == 2){

            print("Option 3");

        }
        else if(currentMenuItem == 3){

            print("Option 4");

        }

        Pause();
        

    }

    public void keyAction(){

        if(Input.GetKeyDown(KeyCode.W)){

            buttons[0].sceneImage.color = buttons[0].PressedColor;
            print("Option 1");

        }

        else if(Input.GetKeyDown(KeyCode.D)){

            buttons[1].sceneImage.color = buttons[1].PressedColor;
            print("Option 2");

        }

        else if(Input.GetKeyDown(KeyCode.S)){

            buttons[2].sceneImage.color = buttons[2].PressedColor;
            print("Option 3");
        }

        else if(Input.GetKeyDown(KeyCode.A)){

            buttons[3].sceneImage.color = buttons[3].PressedColor;
            print("Option 4");
        }

        Pause();

    }

    public void Pause(){

        if(canvas.gameObject.activeInHierarchy == false){

            canvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            print("Menu open");

        }

        else{

            canvas.gameObject.SetActive(false);
            Time.timeScale = 1;
            print("Menu closed");

        }

    }

}

[System.Serializable]
public class MenuButton{

    public string name;
    public Image sceneImage;
    public Color NormalColor = Color.white;
    public Color HighlightedColor = Color.grey;
    public Color PressedColor = Color.gray;

}