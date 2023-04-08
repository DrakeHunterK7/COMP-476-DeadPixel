using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    //UI References
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _controlScreen;
    [SerializeField] private GameObject _teamSelection;
    [SerializeField] private GameObject _shipSelection;
    [SerializeField] private GameObject _mapSelection;

    //Player Data Reference
    private PlayerInformation _playerData;

    //Camera Reference and Variables
    private CameraMovementMenu _camera;
    [SerializeField] private Camera _cameraObject;
    [SerializeField] private GameObject[] _cameraPositions;
    //_cameraPositions = { mainMenu, teamSelection, shipSelection_1, shipSelection_2, shipSelection_3, mapSelection }

    //GameObject References
    [SerializeField] private GameObject[] _motherships;
    [SerializeField] private GameObject[] _ships;
    [SerializeField] private GameObject _shipSelected;

    //Selection Variables
    private int _teamSelected;
    private int _shipTypeSelected;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovementMenu>();
        _playerData = new PlayerInformation();
    }

    public void GoToMainMenu(string from)
    {
        switch(from)
        {
            case "Controls":
                _controlScreen.SetActive(false);
                _mainMenu.SetActive(true);
                break;

            case "TeamSelection":
                _teamSelection.SetActive(false);
                _mainMenu.SetActive(true);
                _camera.SetCameraTarget(_cameraPositions[0]);
                break;
        }
    }

    public void GoToTeamSelection(string from)
    {
        switch (from)
        {
            case "MainMenu":
                _mainMenu.SetActive(false);
                _teamSelection.SetActive(true);
                _camera.SetCameraTarget(_cameraPositions[1]);
                break;

            case "ShipSelection":
                _shipSelection.SetActive(false);
                _teamSelection.SetActive(true);
                _camera.SetCameraTarget(_cameraPositions[1]);

                foreach (GameObject ships in _ships)
                {
                    ships.SetActive(false);
                }
                break;
        }
    }

    public void GoToShipSelection(string from)
    {
        bool updateTeam = true;

        //UI Switch
        switch (from)
        {
            case "MapSelection":
                //Hide Ship Selected Model
                _shipSelected.transform.GetChild(_teamSelected).gameObject.SetActive(false);
                _shipSelected.transform.GetChild(_teamSelected).gameObject.transform.GetChild(_shipTypeSelected).gameObject.SetActive(false);
                //Update UI
                _mapSelection.SetActive(false);
                _shipSelection.SetActive(true);
                //Update Camera Position
                _camera.SetCameraTarget(_cameraPositions[_teamSelected + 2]);
                _ships[_teamSelected].SetActive(true);
                updateTeam = false;
                break;

            case "Blue":
                _teamSelection.SetActive(false);
                _shipSelection.SetActive(true);
                _camera.SetCameraTarget(_cameraPositions[2]);
                _ships[0].SetActive(true);
                _teamSelected = 0;
                break;

            case "Red":
                _teamSelection.SetActive(false);
                _shipSelection.SetActive(true);
                _camera.SetCameraTarget(_cameraPositions[3]);
                _ships[1].SetActive(true);
                _teamSelected = 1;
                break;

            case "Yellow":
                _teamSelection.SetActive(false);
                _shipSelection.SetActive(true);
                _camera.SetCameraTarget(_cameraPositions[4]);
                _ships[2].SetActive(true);
                _teamSelected = 2;
                break;
        }

        //Update Player Data if a team is selected
        if (updateTeam)
            _playerData.SetTeam(_teamSelected);
    }

    public void GoToMapSelection(string from)
    {
        //Hide the ships
        _ships[_teamSelected].SetActive(false);

        //UI Switch
        switch (from)
        {
            case "Attack":
                //Update UI
                _shipSelection.SetActive(false);
                _mapSelection.SetActive(true);
                //Update Camera Position
                _camera.SetCameraTarget(_cameraPositions[5]);
                //Update
                _shipTypeSelected = 0;
                break;

            case "Speed":
                _shipSelection.SetActive(false);
                _mapSelection.SetActive(true);
                _camera.SetCameraTarget(_cameraPositions[5]);
                _shipTypeSelected = 1;
                break;

            case "Defense":
                _shipSelection.SetActive(false);
                _mapSelection.SetActive(true);
                _camera.SetCameraTarget(_cameraPositions[5]);
                _shipTypeSelected = 2;
                break;
        }

        //Update Player Data
        _playerData.SetShipType(_shipTypeSelected);
        Debug.Log(_playerData);

        //Reveal Ship Selected Model
        _shipSelected.transform.GetChild(_teamSelected).gameObject.SetActive(true);
        _shipSelected.transform.GetChild(_teamSelected).gameObject.transform.GetChild(_shipTypeSelected).gameObject.SetActive(true);
    }

    public void GetControls(string from)
    {
        switch (from)
        {
            case "MainMenu":
                _mainMenu.SetActive(false);
                _controlScreen.SetActive(true);
                break;

            case "PauseMenu":
                _pauseMenu.SetActive(false);
                _controlScreen.SetActive(true);
                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
