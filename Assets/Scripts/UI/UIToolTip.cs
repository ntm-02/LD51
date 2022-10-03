using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIToolTip : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        SetText(-1); // send to default
    }

    // Buttons are funny. Integer sets the text we want.
    public void SetText(int option)
    {
        switch(option)
        {
            case 0: // Hovering over Move
                text.text = "Press to enable movement";
                break;
            case 1: // Move mode
                text.text = "Click on the grid to move.";
                break;
            case 2: // Item View
                text.text = "View your current consumable items.";
                break;
            case 3: // Select an item to consume
                text.text = "Select an item to consume.";
                break;
            case 4: // Skill hover
                text.text = "View available skills.";
                break;
            case 5: // Skill view
                text.text = "Select a skill to use.";
                break;
            case 6: // Ends your turn.
                text.text = "End your turn.";
                break;
            case 7:
                text.text = "Return to the previous menu";
                break;
            // Attack Texts are from this point under -------------------------------
            case 20: // Slash
                text.text = "Deal 40 Damage in a short wide-range. Costs 2 seconds.";
                break;
            case 21: // Arrow
                text.text = "Deal 20 Damage in a long line. Costs 5 seconds.";
                break;
            case 22: // Airstrike
                text.text = "Deal 80 damage to a single target. Costs 8 seconds.";
                break;
            case 23: // Defend
                text.text = "reduce damage -- dave pls add";
                break;
            // Item Texts are from this point under ---------------------------------
            case 60:
                text.text = "Consume a health potion to heal 20 HP.";
                break;
            default:
                text.text = "Select an action or end your turn.";
                break;
        }
    }
}
