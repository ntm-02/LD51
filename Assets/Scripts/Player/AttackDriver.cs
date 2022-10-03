using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackDriver : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    private GameObject gameObj;
    private int attackAmount;
    public int timeCost = 0;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        gameObj = boxCollider.gameObject;
        boxCollider.enabled = false;
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
    public void ChangeColliderX(float x)
    {
        //boxCollider.gameObject.transform.localScale.x == new ;
    }

    public void ChangeColliderY(float y)
    {

    }

    public void toggleActive()
    {
        gameObj.SetActive(!gameObj.active);
    }

    public void SetTimeCost(int newCost)
    {
        timeCost = newCost;
    }

    public void TryAttack()
    {
        Debug.Log("Called tryattack");
        if (timeCost <= PlayerTime.currPlayerTime)
        {
            //Debug.Log("Success");
            StartCoroutine(PerformAttack());
            
        } else
        {
            Debug.Log("You broke asf");
        }
    }

    private IEnumerator PerformAttack()
    {
        boxCollider.enabled = true;
        yield return new WaitForSeconds(1);
        boxCollider.enabled = false;
        PlayerTime.currPlayerTime -= timeCost;

    }

    public void ToggleButton(GameObject obj)
    {
        Button button = obj.GetComponent<Button>();

        if (button.enabled)
        {
            button.enabled = false;
            // disable object
            obj.GetComponent<Image>().color = Color.black;
        }
        else
        {
            button.enabled = true;
            obj.GetComponent<Image>().color = Color.white;

        }
    }

    public void ButtonOff(GameObject obj)
    {
        Button button = obj.GetComponent<Button>();
        button.enabled = false;
        // disable object
        obj.GetComponent<Image>().color = Color.black;
        gameObject.SetActive(false);
    }
}
