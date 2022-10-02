using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerCombat : MonoBehaviour, IKillable
{
    BoxCollider2D boxCollider;
    DamageableComponent damageableComponent;
    [SerializeField] int damagePerHit = 10;
    [SerializeField] private GameObject damageLight;

    void Start()
    {
        damageableComponent = this.gameObject.AddComponent<DamageableComponent>();
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        damageLight.SetActive(false);
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
        if (!damageLight.GetComponent<Light2D>().isActiveAndEnabled)
        {
            damageLight.SetActive(true);
        }

        damageLight.SetActive(true);
        yield return new WaitForSeconds(.5f);
        damageLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
