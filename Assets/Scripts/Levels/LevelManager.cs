using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject successCanvas;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        successCanvas.SetActive(false);
        // wait a long time to make sure the ending tile is spawned in first
        yield return new WaitForSeconds(0.1f);
        transform.position = GameManager.EndingTile.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("reached finish!");
            successCanvas.SetActive(true);
            // display success canvas, similar to how death canvas works
            // reload scene
        }
    }
}
