using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerCombat : MonoBehaviour, IKillable
{
    BoxCollider2D boxCollider;
    DamageableComponent damageableComponent;
    Tile top, right, bottom, left;
    [SerializeField] int damagePerHit = 10;
    [SerializeField] private Light2D damageLight;

    void Start()
    {
        damageableComponent = this.gameObject.AddComponent<DamageableComponent>();
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        damageLight.enabled = false;
    }

    public static void UpdateTiles()
    {
        
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
        
    }
}
