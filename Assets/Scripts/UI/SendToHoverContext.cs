using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SendToHoverContext : MonoBehaviour
{
    private HoverContext hoverContext;
    private Tile tileScript;



    public void SetContextMenu(int state)
    {
        if (hoverContext == null) {
            hoverContext = FindObjectOfType<HoverContext>();
        }
        if (tileScript == null) {
            tileScript = GetComponent<Tile>();
        }

        if (tileScript.hasEnemy)
        {
            // do special behavior for what type of enemy
            hoverContext.UpdateContext(10); // 10 refers to slime enemy type
            GameObject enemyObj = tileScript.GetCollisionObject();
            DamageableComponent dmgComp = enemyObj.GetComponentInChildren<DamageableComponent>();
            hoverContext.currentEnemHP = dmgComp.GetCurrentHealth();
            //hoverContext.currentEnemHPMax = dmgComp.
        }
        else
        {
            hoverContext.UpdateContext(state);

        }
        //Debug.Log("Sent!");
    }

}
