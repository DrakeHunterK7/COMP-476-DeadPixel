using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInformation
{
    //Speed Variables
    public float _attackSpeed;
    public float _movementSpeed;

    //Attack Variables
    public float _attackStrength;

    //Defense Variables
    public float _hp;
    public float _shieldHp;

    //Team and Ship Selection Variables
    public int _team;
    public int _shipType;

    //Default Player Constructor
    public ShipInformation()
    {
        //Default: Blue team -- Attack ship
        _team = 0;
        _shipType = 0;

        //Default Ship Stats: Attack = 15 ; Speed = 15 ; Defense = 15
        //Attack
        _attackStrength = 15f;

        //Speed
        _attackSpeed = 15f;
        _movementSpeed = 15f;

        //Defense
        _hp = 100f;
        _shieldHp = 50f;
    }

    public ShipInformation(int team, int shipType)
    {
        _team = team;
        _shipType = shipType;

        int attackMultiplier;
        int speedMultiplier;
        int defenseMultiplier;

        switch (shipType)
        {
            case 0: //Attack Build
                attackMultiplier = 2;
                speedMultiplier = 1;
                defenseMultiplier = 1;
                break;

            case 1: //Speed Build
                attackMultiplier = 1;
                speedMultiplier = 2;
                defenseMultiplier = 1;
                break;

            case 2: //Defense Build
                attackMultiplier = 1;
                speedMultiplier = 1;
                defenseMultiplier = 2;
                break;

            default: //Default Build
                attackMultiplier = 1;
                speedMultiplier = 1;
                defenseMultiplier = 1;
                break;
        }

        //Ship Stats: Attack = 15 * multiplier ; Speed = 15 * multiplier ; Defense = 15 * multiplier
        //Attack
        _attackStrength = 15f * attackMultiplier;

        //Speed
        _attackSpeed = 15f * speedMultiplier;
        _movementSpeed = 15f * speedMultiplier;

        //Defense
        _hp = 100f;
        _shieldHp = 50f * defenseMultiplier;
    }
    public void SetTeam(int team)
    {
        _team = team;
    }

    public void SetShipType(int shipType)
    {
        _shipType = shipType;

        int attackMultiplier;
        int speedMultiplier;
        int defenseMultiplier;

        switch (shipType)
        {
            case 0: //Attack Build
                attackMultiplier = 2;
                speedMultiplier = 1;
                defenseMultiplier = 1;
                break;

            case 1: //Speed Build
                attackMultiplier = 1;
                speedMultiplier = 2;
                defenseMultiplier = 1;
                break;

            case 2: //Defense Build
                attackMultiplier = 1;
                speedMultiplier = 1;
                defenseMultiplier = 2;
                break;

            default: //Default Build
                attackMultiplier = 1;
                speedMultiplier = 1;
                defenseMultiplier = 1;
                break;
        }

        //Ship Stats: Attack = 15 * multiplier ; Speed = 15 * multiplier ; Defense = 15 * multiplier
        //Attack
        _attackStrength = 15f * attackMultiplier;

        //Speed
        _attackSpeed = 15f * speedMultiplier;
        _movementSpeed = 15f * speedMultiplier;

        //Defense
        _hp = 100f;
        _shieldHp = 50f * defenseMultiplier;
    }

    public override string ToString()
    {
        return "Team : " + _team + ", Ship Type: " + _shipType + 
            "\nHP : " + _hp + ", ATK : " + _attackStrength + ", ATK-SPD : " + _attackSpeed 
            + ", SPD : " + _movementSpeed + ", DEF : " + _shieldHp;
    }
}
