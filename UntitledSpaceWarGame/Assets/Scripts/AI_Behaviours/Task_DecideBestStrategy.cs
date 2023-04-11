using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using System.Linq;

/* Strategy A -- Defend the mothership
 * Strategy B -- Attack Enemy mothership A
 * Strategy C -- Attack Enemy mothership B
 * Strategy D -- Repair health 
 * Strategy E -- IDK lol
 * */

public class Task_DecideBestStrategy : Node
{
    Mothership _teamMotherShip;
    Mothership _enemyMotherShipA;
    Mothership _enemyMotherShipB;

    Transform _transform;
    ShipInformation _shipInformation;
    ShipAIBT _shipAIBT;

    float _maxHP = 100.0f;

    public Task_DecideBestStrategy(Transform transform, ShipInformation shipInfo, ShipAIBT shipAIBT)
    {
        _transform = transform;
        _shipInformation = shipInfo;
        _shipAIBT = shipAIBT;

        // Set mothership variables for reference
        GameObject[] motherShips = GameObject.FindGameObjectsWithTag("Mothership");

        foreach (GameObject ship in motherShips)
        {
            Mothership mothershipInfo = ship.GetComponent<Mothership>();

            if (mothershipInfo._team == _shipInformation._team)
            {
                _teamMotherShip = mothershipInfo;
            }
            else
            {
                if (_enemyMotherShipA == null)
                {
                    _enemyMotherShipA = mothershipInfo;
                }
                else
                {
                    _enemyMotherShipB = mothershipInfo;
                }
            }
        }
        _shipAIBT = shipAIBT;
    }

    public override NodeState Evaluate()
    {
        float bestScore = 0.0f;
        float[] strategyScores =
        {
            0.0f, // CalculateStrategyA(),
            CalculateStrategyB(),
            CalculateStrategyC(),
            CalculateStrategyD()
        };

        for(int i = 0; i < 4; i++)
        {
            if (strategyScores[i] > bestScore)
            {
                switch(i)
                {
                    case (0):
                        _shipAIBT.SetRootData("PathfindEndPosition", _teamMotherShip.transform.position);
                        _shipAIBT.SetRootData("NewStrategy", 'A');
                        break;
                    case (1):
                        _shipAIBT.SetRootData("PathfindEndPosition", _enemyMotherShipA.transform.position);
                        _shipAIBT.SetRootData("NewStrategy", 'B');
                        break;
                    case (2):
                        _shipAIBT.SetRootData("PathfindEndPosition", _enemyMotherShipB.transform.position);
                        _shipAIBT.SetRootData("NewStrategy", 'C');
                        break;
                    case (3):
                        _shipAIBT.SetRootData("PathfindEndPosition", _teamMotherShip.transform.position);
                        _shipAIBT.SetRootData("NewStrategy", 'D');
                        break;
                }
                bestScore = strategyScores[i];
            }
        }

        state = NodeState.RUNNING;
        return state;
    }


    // Strategy A
    // Deciding factors:
    //  - Mothership health
    //  - Enemies near Mothership
    private float CalculateStrategyA()
    {
        float score = 0.0f;

        // Team motheship health
        score += _teamMotherShip._currentHealth / Mothership._maxHealth;                                                                        // Current AI health
           
        // Enemies near team mothership
        Collider[] shipsNearMS = Physics.OverlapSphere(_teamMotherShip.transform.position, 200.0f, 1 << 6);
        int nearbyEnemies = 0;

        // Idk if I should be doing this but I am going to do it anyways :P
        foreach(Collider ship in shipsNearMS)
        {
            if(ship.GetComponent<ShipAIBT>().shipInformation._team != _shipInformation._team)
            {
                nearbyEnemies++;
            }
        }

        score += (10 - nearbyEnemies) / 10; // Change 10 to the total number of enemies in the scene

        //score += (20000 - Vector3.Distance(_transform.position, _teamMotherShip.transform.position)) / 20000.0f;                                // Distance calculation


        return score/2.0f;
    }

    // Strategy B
    // Deciding factors
    //  - Enemy mothership health
    //  - Enemies near mothership
    //  - AI current health
    private float CalculateStrategyB()
    {
        float score = 0.0f;

        // Enemy mothership health
        score += _enemyMotherShipA._currentHealth / Mothership._maxHealth;                                                                        // Current AI health
                                                                                                                                                // Enemies near mothership

        // Enemies near Mothership A
        Collider[] shipsNearMS = Physics.OverlapSphere(_enemyMotherShipA.transform.position, 200.0f, 1 << 6);
        int nearbyEnemies = 0;

        // Idk if I should be doing this but I am going to do it anyways :P
        foreach (Collider ship in shipsNearMS)
        {
            if (ship.GetComponent<ShipAIBT>().shipInformation._team != _shipInformation._team)
            {
                nearbyEnemies++;
            }
        }

        score += (10 - nearbyEnemies) / 10; // Change 10 to the total number of enemies in the scene

        // AI Current Health
        score += _shipInformation._hp / 100.0f;



        return score / 3.0f;
    }

    // Strategy B
    // Deciding factors
    //  - Enemy mothership health
    //  - Enemies near mothership
    //  - Distance to enemy mothership
    //  - AI current health
    private float CalculateStrategyC()
    {
        float score = 0.0f;

        // Enemy mothership health
        score += _enemyMotherShipB._currentHealth / Mothership._maxHealth;                                                                        // Current AI health
                                                                                                                                                  // Enemies near mothership

        // Enemies near Mothership A
        Collider[] shipsNearMS = Physics.OverlapSphere(_enemyMotherShipB.transform.position, 200.0f, 1 << 6);
        int nearbyEnemies = 0;

        // Idk if I should be doing this but I am going to do it anyways :P
        foreach (Collider ship in shipsNearMS)
        {
            if (ship.GetComponent<ShipAIBT>().shipInformation._team != _shipInformation._team)
            {
                nearbyEnemies++;
            }
        }

        score += (10 - nearbyEnemies) / 10; // Change 10 to the total number of enemies in the scene

        // AI Current Health
        score += _shipInformation._hp / 100.0f;


        return score / 3.0f;
    }

    // Strategy B
    // Deciding factors
    //  - Current Health of AI
    //  - Enemies near mothership
    private float CalculateStrategyD()
    {
        float score = 0.0f;


        // AI Current Health
        score += (100 -  _shipInformation._hp) / 100.0f;

        // Enemies near team mothership
        Collider[] shipsNearMS = Physics.OverlapSphere(_teamMotherShip.transform.position, 200.0f, 1 << 6);
        int nearbyEnemies = 0;

        // Idk if I should be doing this but I am going to do it anyways :P
        foreach (Collider ship in shipsNearMS)
        {
            if (ship.GetComponent<ShipAIBT>().shipInformation._team != _shipInformation._team)
            {
                nearbyEnemies++;
            }
        }

        score += (10 - nearbyEnemies) / 10; // Change 10 to the total number of enemies in the scene


        return score / 2.0f;
    }



}
