using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour, IKillable 
{
    UnityEngine.Rendering.Universal.Light2D damageLight;
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
}
