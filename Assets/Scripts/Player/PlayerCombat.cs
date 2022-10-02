using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerCombat : MonoBehaviour, IKillable
{
    BoxCollider2D boxCollider;
    DamageableComponent damageableComponent;
    GameObject[] neighborTiles;
    [SerializeField] int damagePerHit = 10;
    [SerializeField] private Light2D damageLight;

    void Start()
    {
        damageableComponent = this.gameObject.AddComponent<DamageableComponent>();
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        damageLight.enabled = false;
    }

    public void UpdateTiles()
    {
        if (!GameManager.IsPlayerMoving)
        {
            neighborTiles = TilePathFinding.adjacentToPoint(FindObjectOfType<TilePathFinding>().getGrid(), GameManager.PlayerGridPos);
            //foreach (GameObject tile in neighborTiles)
            //{
            //    Debug.Log(tile.GetComponent<Tile>().GetCollisionObject().name);
            //}
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            damageableComponent.TakeDamage(damagePerHit);
        }
    }

    public void Die()
    {
        GameManager.IsPlayerDead = true;
        Destroy(gameObject);
    }

    public void NotifyDamage()
    {
        StartCoroutine(DamageLightToggle());
    }

    IEnumerator DamageLightToggle()
    {
        damageLight.enabled = true;
        yield return new WaitForSeconds(.5f);
        damageLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTiles();
    }
}
