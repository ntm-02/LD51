using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour, IKillable 
{
    UnityEngine.Rendering.Universal.Light2D damageLight;
    [SerializeField] int damagePerHit = 10;

    public void Die()
    {
        Destroy(gameObject);
    }

    public GameObject LoadPrefab()
    {
        return Resources.Load("Slime Enemy") as GameObject;
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

    // Start is called before the first frame update
    void Start()
    {
        damageLight = GetComponentInParent<UnityEngine.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("test");
            collision.gameObject.GetComponent<DamageableComponent>().TakeDamage(damagePerHit); // this should be fine as long as you dont contact before start or something
        } else
        {
           // Debug.Log("no");
        }
    }
}
