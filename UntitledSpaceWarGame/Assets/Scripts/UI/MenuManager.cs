using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //UI References
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _controlMenu;
    [SerializeField] private GameObject _playerHUD;
    [SerializeField] bool canPause;

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
        _pauseMenu.SetActive(true);
        _playerHUD.SetActive(false);
    }

    public void Resume()
    {
        //Resume game and hide/lock cursor
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _playerHUD.SetActive(true);
    }

    public void ToggleControls()
    {
        if (_controlMenu.activeSelf)
        {
            _controlMenu.SetActive(false);
            _pauseMenu.SetActive(true);
        }
        else
        {
            _controlMenu.SetActive(true);
            _pauseMenu.SetActive(false);
        }
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
