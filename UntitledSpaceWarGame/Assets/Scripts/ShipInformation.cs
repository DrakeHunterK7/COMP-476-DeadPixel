using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInformation
{
    //Speed Variables
    public float _movementSpeed;

    //Attack Variables
    public float _attackStrength;
    public float _energyLevel;

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
        _movementSpeed = 200f;

        //Defense
        _hp = 100f;
        _shieldHp = 100f;

        //Energy
        _energyLevel = 100f;
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

        //Ship Stats: Attack = 15 * multiplier ; Speed = 100 * multiplier ; Defense = 15 * multiplier
        //Attack
        _attackStrength = 15f * attackMultiplier;

        //Speed
        _attackSpeed = 15f * speedMultiplier;
        _movementSpeed = 200f * speedMultiplier;

        //Defense
        _hp = 100f;
        _shieldHp = 100f * defenseMultiplier;

        //Energy
        _energyLevel = 100f;
    }

    public ShipInformation(ShipInformation shipData)
    {
        _team = shipData._team;
        _shipType = shipData._shipType;

        //Attack
        _attackStrength = shipData._attackStrength;

        //Speed
        _movementSpeed = shipData._movementSpeed;

        //Defense
        _hp = shipData._hp;
        _shieldHp = shipData._shieldHp;

        //Energy
        _energyLevel = shipData._energyLevel;
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
        _movementSpeed = 200f * speedMultiplier;

        //Defense
        _hp = 100f;
        _shieldHp = 100f * defenseMultiplier;

        //Energy
        _energyLevel = 100f;
    }

    public void UseEnergy()
    {
        _energyLevel = Mathf.Clamp(_energyLevel - 0.1f, 0f, 100f);
    }

    public void ReplenishEnergy()
    {
        _energyLevel = Mathf.Clamp(_energyLevel + 0.05f, 0f, 100f);
    }

    public bool TakeDamage(float AttackForce)
    {
        if (_shieldHp > AttackForce)
        {
            _shieldHp -= AttackForce;
            AttackForce = 0;
        }
        else if (_shieldHp > 0)
        {
            AttackForce -= _shieldHp;
            _shieldHp = 0;
        }

        if (_hp > AttackForce)
        {
            _hp -= AttackForce;
        }
        else
        {
            //SHIP DESTROYED
            _hp = 0;
            return true;
        }

        return false;
    }

    public int GetTeam()
    {
        return _team;
    }

    public int GetShipType()
    {
        return _shipType;
    }

    public float GetHP()
    {
        return _hp;
    }

    public float GetShieldHP()
    {
        return _shieldHp;
    }

    public float GetEnergyLevel()
    {
        return _energyLevel;
    }

    public float GetAttackForce()
    {
        return _attackStrength;
    }

    public override string ToString()
    {
        return "Team : " + _team + ", Ship Type: " + _shipType + 
            "\nHP : " + _hp + ", ATK : " + _attackStrength + ", SPD : " + _movementSpeed + ", DEF : " + _shieldHp + ", ENERGY : " + _energyLevel;
    }
}
