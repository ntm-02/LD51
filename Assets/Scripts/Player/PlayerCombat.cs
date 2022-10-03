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

    private bool firstTime = true;  // this wil stop us from trying to access the grid before it exists

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
            //print(GameManager.PlayerGridPos);
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

    public void EndPlayerTurn()
    {
        PlayerTime.currPlayerTime = 0f;
        GameManager.IsPlayerTurn = false;
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

    IEnumerator UpdateTileWait()
    {
        yield return new WaitForSeconds(0.1f);
        firstTime = false;
        if (firstTime)
        {
            UpdateTiles();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (firstTime)
        {
            StartCoroutine(UpdateTileWait());
        }
        else
        {
            UpdateTiles();
        }
    }
}
