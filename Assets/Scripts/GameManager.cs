using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //// specify important game states here ////
    public static bool IsPlayerTurn = true;
    public static bool IsPlayerDead = false;
    public static bool HasEscaped = false;
    public static bool IsReloadingLevel = false;
    public static bool IsPlayerMoving = false;
    public static bool IsEnemyMoving = false;
    public static bool InCutscene = false;

    public Action action = Action.none; // Controlled by buttons in the UI canvas.

    public enum Action
    {
        moveMode,
        attacking,
        none
    }

    // reference to grid of enemy/interactable locations

    public static Tile EndingTile;//can be changed to GameObject if necessary. change endingTile.cs line 10 just the gameObject.

    private static Vector2 _PlayerGridPos = Vector2.zero;

    public static Vector2 PlayerGridPos
    {
        get => _PlayerGridPos;
        set
        {
            _PlayerGridPos = value;
            PlayerTileBasedMovement.setGridPos(value);
        }
    }

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
        //PlayerGridPos = new Vector2(10, 10);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerDeath()
    {
        IsPlayerDead = true;
        Time.timeScale = 0;
        DeathCanvas.SetActive(true);

    }
    public void ReloadScene()
    {
        DeathCanvas.SetActive(false);
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Buttons are funny! so we cant use enum.
    // int state - Desired state to toggle. If action is already in this state, swap it to none.
    // 0 - move
    // 1 - attack
    // 2 - none
    public void ToggleAction(int state) {
        Action newAction;
        switch(state)
        {
            case 0:
                newAction = Action.moveMode;
                break;
            case 1:
                newAction = Action.attacking;
                break;
            case 2:
                newAction = Action.none;
                break;
            default:
                Debug.Log("button was set up improperly - gave GameManager.SetAction() an invalid int. Valid 0-2. Set to Default - none");
                newAction = Action.none;
                break;
        }
        if (action == newAction)
        {
            action = Action.none;
        } else
        {
            action = newAction;
        }
    }

    public void SetAction(int state)
    {
        Action newAction;
        switch (state)
        {
            case 0:
                newAction = Action.moveMode;
                break;
            case 1:
                newAction = Action.attacking;
                break;
            case 2:
                newAction = Action.none;
                break;
            default:
                Debug.Log("button was set up improperly - gave GameManager.SetAction() an invalid int. Valid 0-2. Set to Default - none");
                newAction = Action.none;
                break;
        }
       
        action = newAction;
        
    }
}
