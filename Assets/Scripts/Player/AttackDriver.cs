using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDriver : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    private GameObject gameObj;
    int attackAmount;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        gameObj = boxCollider.gameObject;
        gameObj.SetActive(false);
    }

    public void SetIsAttackColliderOn(bool boolean)
    {
        boxCollider.enabled = boolean;
    }

    public void SetAttackAmount(int amount)
    {
        attackAmount = amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInChildren<DamageableComponent>().TakeDamage(attackAmount);
        }
    }
    private void ChangeColliderX(float x)
    {
        //boxCollider.gameObject.transform.localScale.x == new ;
    }

    private void ChangeColliderY(float y)
    {

    }
}
