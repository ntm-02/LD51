using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, IKillable
{
    DamageableComponent damageableComponent;
    [SerializeField] int damagePerHit = 10;
    [SerializeField] private GameObject damageLight;

    void Start()
    {
        damageableComponent = this.gameObject.AddComponent<DamageableComponent>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<DamageableComponent>(out DamageableComponent target))
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
        damageLight.SetActive(true);
        yield return new WaitForSeconds(.5f);
        damageLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
