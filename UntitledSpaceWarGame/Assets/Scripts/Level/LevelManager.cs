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
    [Header ("Agent Model")]
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private GameObject _aiModel;
    private ShipInformation _player;
    private ShipController _playerController;

    //Team Positions
    [Header("Ships/MotherShips Variables")]
    [SerializeField] private GameObject[] _motherships;
    [SerializeField] private Transform[] _mothershipPositions;
    [SerializeField] private Transform[] _allyPositions;
    [SerializeField] private Transform[] _enemyPositions_1;
    [SerializeField] private Transform[] _enemyPositions_2;
    [SerializeField] private Color _teamColor;
    [SerializeField] private Color _enemyColor;

    //Debug Team/Ship Choice
    [Header("Manual Team/Ship Selection")]
    [SerializeField] private int _teamSelected;
    [SerializeField] private int _shipTypeSelected;

    // Start is called before the first frame update
    void Start()
    {
        if (Time.timeScale == 1) Time.timeScale = 0;

        if (GameObject.FindGameObjectsWithTag("CharacterData").Length == 0)
        {
            _player = new ShipInformation(_teamSelected, _shipTypeSelected);
        }
        else
            _player = GameObject.FindGameObjectsWithTag("CharacterData")[0].GetComponent<PlayerData>().GetShipData();

        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();
        _playerModel.GetComponent<ShipController>().SetShipData(_player);

        UpdateAgentModel(_playerModel, true);
        UpdateTeamPositions();
        SpawnAgents();
        StartCountdown();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerIsRunning)
            UpdateGameTimer();
    }

    public void UpdateAgentModel(GameObject agentModel, bool isAlly)
    {
        int team = 0;
        int shipType = 0;

        //Update Outline Color
        if (!agentModel.CompareTag("Player"))
        {
            team = agentModel.GetComponent<ShipAIBT>().GetShipData().GetTeam();
            shipType = agentModel.GetComponent<ShipAIBT>().GetShipData().GetShipType();

            if (isAlly)
                agentModel.GetComponent<Outline>().SetColor(_teamColor);
            else
                agentModel.GetComponent<Outline>().SetColor(_enemyColor);

            agentModel.GetComponent<Outline>().enabled = true;

            for (int i = 0; i<3; i++)
            {
                if (team != i)
                    agentModel.transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int j = 0; j<3; j++)
            {
                if (shipType != j)
                    agentModel.transform.GetChild(team).gameObject.transform.GetChild(j).gameObject.SetActive(false);
            }
        }
        else
        {
            team = agentModel.GetComponent<ShipController>().GetShipData().GetTeam();
            shipType = agentModel.GetComponent<ShipController>().GetShipData().GetShipType();

            //Update Team Model
            agentModel.transform.GetChild(team).gameObject.SetActive(true);

            //Update Ship Type Model
            agentModel.transform.GetChild(team).gameObject.transform.GetChild(shipType).gameObject.SetActive(true);
        }
    }

    public void UpdateTeamPositions()
    {
        int team = _player.GetTeam();
        int swapedTeam = team == 1 ? 1 : 2;
        if (team != 0)
        {
            //Update Ship Positions
            _motherships[0].transform.position = _mothershipPositions[swapedTeam].position;
            _motherships[swapedTeam].transform.position = _mothershipPositions[0].position;

            //Update Ship Outline Colors
            _motherships[0].GetComponent<Outline>().SetColor(_enemyColor);
            _motherships[swapedTeam].GetComponent<Outline>().SetColor(_teamColor);

        } //BLUE TEAM CHOSEN (NO CHANGES NEEDED)
    }

    public void SpawnAgents()
    {
        //TODO: ADD SHIP INFORMATION TO AI SHIPS
        int team = _player.GetTeam();
        int enemyTeam_1 = team == 1 ? 0 : 1;
        int enemyTeam_2 = team == 2 ? 0 : 2;

        for (int i = 0; i < 4; i++) //PLAYER TEAM
        {
            int shipType = Random.Range(0, 3);
            GameObject ally = Instantiate(_aiModel, _allyPositions[i].transform.position, _allyPositions[i].transform.rotation);
            _motherships[team].GetComponent<Mothership>()._teamShips.Add(ally);
            var allyScript = ally.GetComponent<ShipAIBT>();
            allyScript.SetShipData(team, shipType);
            UpdateAgentModel(ally, true);
        }

        //Spawn Enemies
        for (int j = 0; j < 5; j++) //ENEMY TEAM 1
        {
            int shipType = Random.Range(0, 3);
            GameObject enemy = Instantiate(_aiModel, _enemyPositions_1[j].transform.position, _enemyPositions_1[j].transform.rotation);
            enemy.GetComponent<ShipAIBT>().SetShipData(enemyTeam_1, shipType);
            _motherships[enemyTeam_1].GetComponent<Mothership>()._teamShips.Add(enemy);
            UpdateAgentModel(enemy, false);
        }

        for (int k = 0; k < 5; k++) //ENEMY TEAM 2
        {
            int shipType = Random.Range(0, 3);
            GameObject enemy = Instantiate(_aiModel, _enemyPositions_2[k].transform.position, _enemyPositions_2[k].transform.rotation);
            enemy.GetComponent<ShipAIBT>().SetShipData(enemyTeam_2, shipType);
            _motherships[enemyTeam_2].GetComponent<Mothership>()._teamShips.Add(enemy);
            UpdateAgentModel(enemy, false);
        }
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
        _playerController._canMove = true;
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
