using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiHealthBar : MonoBehaviour
{
    [SerializeField] Image healthbar;
    private DamageableComponent playerDmgComp;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindObj());
    }

    void Update()
    {
        if (playerDmgComp != null)
        {
            healthbar.fillAmount = playerDmgComp.GetHealthPercentage();
        }
    }

    private IEnumerator FindObj()
    {
        yield return new WaitForSeconds(2);
        playerDmgComp = GameObject.Find("Player").GetComponent<DamageableComponent>();

    }
}
