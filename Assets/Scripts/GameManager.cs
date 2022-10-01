using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //// specify important game states here ////
    public static bool IsPlayerTurn = false;
    public static bool IsPlayerDead = false;
    public static bool HasEscaped = false;
    public static bool IsReloadingLevel = false;
    public static bool InCutscene = false;

    // reference to grid of enemy/interactable locations

    [SerializeField] private GameObject DeathCanvas;
    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameManager>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerDeath()
    {
        Time.timeScale = 0;
        DeathCanvas.SetActive(true);

    }
    public void ReloadScene()
    {
        DeathCanvas.SetActive(false);
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
