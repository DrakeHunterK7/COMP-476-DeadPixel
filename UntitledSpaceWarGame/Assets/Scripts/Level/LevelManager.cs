using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    //Game Timer Variables
    [Header ("Game Timer Variables")]
    public Text _timerText;
    public float _timeRemaining = 301f;
    private string _minutes;
    private string _seconds;
    private bool _timerIsRunning = false;
    private bool _timerDone = false;

    //UI References
    [Header ("UI References")]
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private Text _winnerText;
    [SerializeField] private GameObject _countdownUI;
    [SerializeField] private Text _countdownText;

    //Player Reference
    [Header ("Player Reference")]
    [SerializeField] private GameObject _playerModel;
    private ShipInformation _player;

    //Team Positions
    [Header("Ships/MotherShips Variables")]
    [SerializeField] private GameObject[] _motherships;
    [SerializeField] private Transform[] _mothershipPositions;
    [SerializeField] private Transform[] _allyPositions;
    [SerializeField] private Transform[] _enemyPositions_1;
    [SerializeField] private Transform[] _enemyPositions_2;

    // Start is called before the first frame update
    void Start()
    {
        if (Time.timeScale == 1) Time.timeScale = 0;

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>().GetShipData();

        UpdatePlayerModel();
        UpdateTeamPositions();
        SpawnPlayers();
        StartCountdown();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerIsRunning)
            UpdateGameTimer();
    }

    public void UpdatePlayerModel()
    {
        int team = _player.GetTeam();
        int shipType = _player.GetShipType();

        //Update Team Model
        _playerModel.transform.GetChild(team).gameObject.SetActive(true);

        //Update Ship Type Model
        _playerModel.transform.GetChild(team).gameObject.transform.GetChild(shipType).gameObject.SetActive(true);
    }

    public void UpdateTeamPositions()
    {
        int team = _player.GetTeam();
        if (team != 0)
        {
            if (team == 1)
            {
                //RED TEAM CHOSEN
                _motherships[0].transform.position = _mothershipPositions[1].position;
                _motherships[1].transform.position = _mothershipPositions[0].position;
            }
            else
            {
                //YELLOW TEAM CHOSEN
                _motherships[0].transform.position = _mothershipPositions[2].position;
                _motherships[2].transform.position = _mothershipPositions[0].position;
            }
        } //BLUE TEAM CHOSEN
    }

    public void SpawnPlayers()
    {

    }

    public void StartCountdown()
    {
        _countdownUI.SetActive(true);
        StartCoroutine(Countdown(4));
    }

    IEnumerator Countdown(int seconds)
    {
        Time.timeScale = 0;

        while (seconds > 0)
        {
            if (seconds == 1)
            {
                _countdownText.text = "ATTACK .";
            }
            else
            {
                //Show countdown
                _countdownText.text = (seconds - 1).ToString();
            }

            yield return new WaitForSecondsRealtime(1);
            seconds--;
        }

        _countdownUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        _timerIsRunning = true;
    }

    public void EndGame()
    {

    }

    public void UpdateGameTimer()
    {
        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
            _minutes = Mathf.FloorToInt(_timeRemaining / 60).ToString("00");
            _seconds = Mathf.FloorToInt(_timeRemaining % 60).ToString("00");
        }
        else
        {
            _timeRemaining = 0;
            _minutes = "00";
            _seconds = "00";
            _timerIsRunning = false;
            _timerDone = true;
        }
        //Update Timer text
        _timerText.text = "GAME TIMER\n" + _minutes + " . " + _seconds;
    }
}
