using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timeText.text = PlayerTime.currPlayerTime.ToString();
    }
}
