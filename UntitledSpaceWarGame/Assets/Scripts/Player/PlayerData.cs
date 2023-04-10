using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //Ship Data
    private ShipInformation _shipData;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //Get rid of duplicates
        if (GameObject.FindGameObjectsWithTag("CharacterData").Length != 1)
        {
            GameObject[] duplicates = GameObject.FindGameObjectsWithTag("CharacterData");
            int i = 0;
            foreach (GameObject duplicate in duplicates)
            {
                if (i != 0)
                    Destroy(duplicate);
                i++;
            }
        }
    }

    public ShipInformation GetShipData()
    {
        return _shipData;
    }

    public void SetShipData(ShipInformation shipData)
    {
        _shipData = new ShipInformation(shipData);
    }
}
