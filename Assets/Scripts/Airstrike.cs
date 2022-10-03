using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airstrike : MonoBehaviour
{
    public BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        StartCoroutine(LifeTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInChildren<DamageableComponent>().TakeDamage(80);
        }
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
