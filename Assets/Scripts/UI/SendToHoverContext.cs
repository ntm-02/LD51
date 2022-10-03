using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SendToHoverContext : MonoBehaviour
{
    private HoverContext hoverContext;
    private Tile tileScript;
    // Start is called before the first frame update
    void Start()
    {
        hoverContext = FindObjectOfType<HoverContext>();
        tileScript = GetComponent<Tile>();

    }

    public void SetContextMenu(int state)
    {

        if (tileScript.hasEnemy)
        {
            // do special behavior for what type of enemy
            hoverContext.UpdateContext(10);
            GameObject enemyObj = tileScript.GetCollisionObject();
            // tmw they dont have a healthbar yet

        } else
        {
            hoverContext.UpdateContext(state);

        }
        Debug.Log("Sent!");
    }

}
