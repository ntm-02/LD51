using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject audioSource;
    private bool soundToggle = true;


    // Start is called before the first frame update
    void Start()
    {

    }


    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ToggleSound()
    {
        //soundToggle = !soundToggle;
        //audioSource.SetActive(soundToggle);
    }
    public void toTutorial()
    {
        //SceneManager.LoadScene(1);
    }
    public void ToCredits()
    {
        //SceneManager.LoadScene(3);  // subject to change
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {

    }

}
