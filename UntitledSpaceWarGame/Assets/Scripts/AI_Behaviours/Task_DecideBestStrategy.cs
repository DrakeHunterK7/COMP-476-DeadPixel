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
            CalculateStrategyA(),
            (_enemyMotherShipA == null) ? -100000 : CalculateStrategyB(),
            (_enemyMotherShipB == null) ? -100000 : CalculateStrategyC(),
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

        // Team mothership health
        score += (Mothership._maxHealth - _teamMotherShip._currentHealth);                                                                        // Current AI health
           
        // Enemies near team mothership
        Collider[] shipsNearMS = Physics.OverlapSphere(_teamMotherShip.transform.position, 2000.0f, 1 << 6);
        int nearbyEnemies = 0;

        // Idk if I should be doing this but I am going to do it anyways :P
        foreach(Collider ship in shipsNearMS)
        {
            if (ship.GetComponent<ShipAIBT>() == null) continue;
            if(ship.GetComponent<ShipAIBT>().GetShipData().GetTeam() != _shipInformation._team)
            {
                nearbyEnemies++;
            }
        }

        score += (10 - nearbyEnemies) / 10; // Change 10 to the total number of enemies in the scene

        //score += (20000 - Vector3.Distance(_transform.position, _teamMotherShip.transform.position)) / 20000.0f;

        if (_shipAIBT.mothership != null)
        {
            foreach (var ship in _shipAIBT.mothership.ships)
            {
                if (ship == null || _shipAIBT == null) continue;
                var shipStrategy = ship.GetRootData("NewStrategy");
                if (shipStrategy != null && ship.gameObject != _shipAIBT.gameObject && (char) shipStrategy == 'A')
                {
                    score -= 500f;
                }
            } 
        }
        
        
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
        score += (Mothership._maxHealth - _enemyMotherShipA._currentHealth);                                                                        // Current AI health
                                                                                                                                                // Enemies near mothership

        // Enemies near Mothership A
        Collider[] shipsNearMS = Physics.OverlapSphere(_enemyMotherShipA.transform.position, 2000.0f, 1 << 6);
        int nearbyEnemies = 0;

        // Idk if I should be doing this but I am going to do it anyways :P
        foreach (Collider ship in shipsNearMS)
        {
            if (ship.GetComponent<ShipAIBT>() == null) continue;
            if (ship.GetComponent<ShipAIBT>().GetShipData().GetTeam() != _shipInformation._team)
            {
                nearbyEnemies++;
            }
        }

        score += (10 - nearbyEnemies) / 10; // Change 10 to the total number of enemies in the scene
        
        if (_shipAIBT.mothership != null)
        {
            foreach (var ship in _shipAIBT.mothership.ships)
            {
                var shipStrategy = ship.GetRootData("NewStrategy");
                if (shipStrategy != null && ship.gameObject != _shipAIBT.gameObject && (char) shipStrategy == 'B')
                {
                    score -= 500f;
                }
            } 
        }

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
        score += (Mothership._maxHealth - _enemyMotherShipB._currentHealth);                                                                         // Current AI health
                                                                                                                                                  // Enemies near mothership

        // Enemies near Mothership A
        Collider[] shipsNearMS = Physics.OverlapSphere(_enemyMotherShipB.transform.position, 2000.0f, 1 << 6);
        int nearbyEnemies = 0;

        // Idk if I should be doing this but I am going to do it anyways :P
        foreach (Collider ship in shipsNearMS)
        {
            if (ship.GetComponent<ShipAIBT>() == null) continue;
            if (ship.GetComponent<ShipAIBT>().GetShipData().GetTeam() != _shipInformation._team)
            {
                nearbyEnemies++;
            }
        }

        score += (10 - nearbyEnemies) / 10; // Change 10 to the total number of enemies in the scene
        
        if (_shipAIBT.mothership != null)
        {
            foreach (var ship in _shipAIBT.mothership.ships)
            {
                var shipStrategy = ship.GetRootData("NewStrategy");
                if (shipStrategy != null && ship.gameObject != _shipAIBT.gameObject && (char) shipStrategy == 'C')
                {
                    score -= 500f;
                }
            } 
        }

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
        Collider[] shipsNearMS = Physics.OverlapSphere(_teamMotherShip.transform.position, 2000.0f, 1 << 6);
        int nearbyEnemies = 0;

        // Idk if I should be doing this but I am going to do it anyways :P
        foreach (Collider ship in shipsNearMS)
        {
            if (ship.GetComponent<ShipAIBT>() == null) continue;
            if (ship.GetComponent<ShipAIBT>().GetShipData().GetTeam() != _shipInformation._team)
            {
                nearbyEnemies++;
            }
        }

        score += (10 - nearbyEnemies) / 10; // Change 10 to the total number of enemies in the scene
        
        if (_shipAIBT.mothership != null)
        {
            foreach (var ship in _shipAIBT.mothership.ships)
            {
                var shipStrategy = ship.GetRootData("NewStrategy");
                if (shipStrategy != null && ship.gameObject != _shipAIBT.gameObject && (char) shipStrategy == 'D')
                {
                    score -= 500f;
                }
            } 
        }


        return score / 2.0f;
    }



}
