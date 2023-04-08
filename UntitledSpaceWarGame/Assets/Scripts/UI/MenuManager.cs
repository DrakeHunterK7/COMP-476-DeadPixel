using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //Pause variables
    public GameObject pauseUI;
    [SerializeField] bool canPause;

    private void Start()
    {
        //Correct Time Scale if at Zero
        if (Time.timeScale == 0) Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.P))
                Pause();
        }
    }

    public void Pause()
    {
        //Pause game and unlock/show cursor
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    public void Resume()
    {
        //Resume game and hide/lock cursor
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    //Load the scene from the given name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Quit the app
    public void QuitApp()
    {
        Application.Quit();
    }
}
