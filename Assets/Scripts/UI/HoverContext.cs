using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// After hovering over an object (handled by tiles and the SendToContext.cs script on them)
// Update a little UI menu in the top left displaying information about the given tile or enemy hovered.
// If the image is incorrect, please check the indexes of the imageList field.

public class HoverContext : MonoBehaviour
{
    public int menuShow; // default - none
                         // 10 - Slime Enemy

    [SerializeField] GameObject uiObj;
    [SerializeField] TextMeshProUGUI uiText;
    [SerializeField] List<GameObject> imageList;
                         
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateContext(int state)
    {
        menuShow = state;
        //Debug.Log(state);
        // additional behaviors
        switch(state)
        {
            case 1:
                uiObj.SetActive(true);
                uiText.text = "Tile - Stone. Traversable.";
                DisableImages();
                imageList[0].SetActive(true); // stone
                break;
            case 2:
                uiObj.SetActive(true);
                uiText.text = "Tile - Mud. Traversable.";
                DisableImages();
                imageList[1].SetActive(true); // mud 
                break;
            case 3:
                uiObj.SetActive(true);
                uiText.text = "Tile - Wall. Untraversable.";
                DisableImages();
                imageList[2].SetActive(true);
                break;
            case 10:
                uiObj.SetActive(true);
                uiText.text = "Enemy : Slime. \nHealth ???/XXX";
                DisableImages();
                imageList[3].SetActive(true);
                break;
            default:
                //uiObj.SetActive(false);
                break;
        }
    }

    private void DisableImages()
    {
        foreach (GameObject img in imageList)
        {
            img.SetActive(false);
        }
    }
}
