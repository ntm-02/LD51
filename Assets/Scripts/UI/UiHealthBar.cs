using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UiHealthBar : MonoBehaviour
{
    [SerializeField] Image healthbar;
    [SerializeField] TextMeshProUGUI textMesh;
    private DamageableComponent playerDmgComp;

    // Start is called before the first frame update
    void Start()
    {
        playerDmgComp = GameObject.Find("Player").GetComponent<DamageableComponent>();

        //StartCoroutine(FindObj());
    }

    void Update()
    {
        if (playerDmgComp != null)
        {
            healthbar.fillAmount = playerDmgComp.GetHealthPercentage();
            textMesh.text = playerDmgComp.GetCurrentHealth() + "/100";
        }
    }

    private IEnumerator FindObj()
    {
        yield return new WaitForSeconds(2);
        playerDmgComp = GameObject.Find("Player").GetComponent<DamageableComponent>();

    }
}
