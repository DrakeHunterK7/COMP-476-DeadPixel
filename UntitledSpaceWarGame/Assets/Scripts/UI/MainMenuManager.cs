using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    //UI References
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _controlScreen;
    [SerializeField] private GameObject _teamSelection;
    [SerializeField] private GameObject _shipSelection;
    [SerializeField] private GameObject _mapSelection;

    //Camera Reference and Variables
    private CameraMovementMenu _camera;
    [SerializeField] private Camera _cameraObject;
    [SerializeField] private GameObject[] _cameraPositions;
    //_cameraPositions = { mainMenu, teamSelection, shipSelection_1, shipSelection_2, shipSelection_3, mapSelection }

    //GameObject References
    [SerializeField] private GameObject[] _motherships;
    [SerializeField] private GameObject[] _ships;
    private int _shipMask = 1 >> 6;

    //Selection Variables
    private Transform _highlight;
    private Transform _selection;
    private RaycastHit _raycastHit;
    private bool _isSelectingTeam = false;
    private bool _isSelectingShip = false;
    private int _teamSelected;

    //TODO: Move camera to appropriate ships (of correct team)
    //TODO: Highlight options for team/ship selection

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovementMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        //Select Team or Ship
        if (_isSelectingTeam || _isSelectingShip)
        {
            //Show ships of selected team
            if (_isSelectingShip)
            {
                _ships[_teamSelected].SetActive(true);
            }

            // Highlight Active GameObject
            if (_highlight != null)
            {
                _highlight.gameObject.GetComponent<Outline>().enabled = false;
                _highlight = null;
            }

            Ray ray_option_2 = _cameraObject.ScreenPointToRay(Input.mousePosition);

            var mouseWP = _cameraObject.ScreenToWorldPoint(Input.mousePosition);
            mouseWP.z = _cameraObject.nearClipPlane;
            var rayDir = (mouseWP - _cameraObject.transform.position).normalized;
            Vector3 rayOrigin = _cameraObject.transform.position;

            Ray ray = new Ray(rayOrigin,rayDir*100f);

            Debug.DrawRay(ray.origin, ray.direction * 10000f, Color.red, 100f);

            if (Physics.Raycast(ray_option_2, out _raycastHit, _shipMask)) //Make sure you have EventSystem in the hierarchy before using EventSystem
            {
                _highlight = _raycastHit.transform;
                Debug.Log("target found: " + _highlight.name);

                if (_highlight.tag == "Selectable" && _highlight != _selection)
                {
                    if (_highlight.gameObject.GetComponent<Outline>() != null)
                    {
                        _highlight.gameObject.GetComponent<Outline>().enabled = true;
                    }
                    else
                    {
                        Outline outline = _highlight.gameObject.AddComponent<Outline>();
                        outline.enabled = true;
                    }
                }
                else
                {
                    _highlight = null;
                }
            }

            // Selection of Team or ship
            if (Input.GetMouseButtonDown(0))
            {
                if (_highlight)
                {
                    if (_selection != null)
                    {
                        _selection.gameObject.GetComponent<Outline>().enabled = false;
                    }
                    _selection = _raycastHit.transform;
                    _selection.gameObject.GetComponent<Outline>().enabled = true;
                    _highlight = null;
                }
                else
                {
                    if (_selection)
                    {
                        _selection.gameObject.GetComponent<Outline>().enabled = false;
                        _selection = null;
                    }
                }
            }
        }
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
                _isSelectingTeam = true;
                _camera.SetCameraTarget(_cameraPositions[1]);
                break;

            case "ShipSelection":
                _shipSelection.SetActive(false);
                _teamSelection.SetActive(true);
                _isSelectingShip = false;
                _isSelectingTeam = true;
                _camera.SetCameraTarget(_cameraPositions[1]);

                foreach (GameObject ships in _ships)
                {
                    ships.SetActive(false);
                }
                break;
        }
    }

    public void GoToShipSelection(string from, int teamIndex)
    {
        //UI Switch
        switch (from)
        {
            case "TeamSelection":
                _teamSelection.SetActive(false);
                _shipSelection.SetActive(true);
                _isSelectingTeam = false;
                _isSelectingShip = true;
                break;

            case "MapSelection":
                _mapSelection.SetActive(false);
                _isSelectingShip = true;
                break;
        }

        _camera.SetCameraTarget(_cameraPositions[teamIndex + 2]);
        _ships[teamIndex].SetActive(true);
        _teamSelected = teamIndex;
    }

    public void GetControls()
    {
        _mainMenu.SetActive(false);
        _controlScreen.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
