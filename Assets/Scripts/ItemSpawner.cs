using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [System.Serializable]
    public class WeaponPair
    {
        public GameObject gun;
        public GameObject ammo;
    }

    public List<WeaponPair> weaponPairs;
    public List<Transform> gunSpawnPoints;
    public List<Transform> ammoSpawnPoints;

    public int weaponsToInstantiate = 2; // Voit asettaa t‰m‰n inspectorissa
    public int ammoToInstantiate = 6; // Panosten lukum‰‰r‰, voit asettaa t‰m‰n inspectorissa

    void Start()
    {
        InstantiateWeapons();
    }

    void InstantiateWeapons()
    {
        if (weaponPairs.Count == 0 || gunSpawnPoints.Count == 0 || ammoSpawnPoints.Count == 0)
        {
            Debug.LogError("Weapon pairs list, gun spawn points list, or ammo spawn points list is empty!");
            return;
        }

        int weaponCount = Mathf.Min(weaponsToInstantiate, weaponPairs.Count);

        List<int> selectedIndices = new List<int>();
        List<int> selectedAmmoSpawnIndices = new List<int>();

        for (int i = 0; i < weaponCount; i++)
        {
            int randomIndex = GetRandomIndex(weaponPairs.Count, selectedIndices);

            // Valitse satunnainen spawn-piste aseelle
            Transform gunSpawnPoint = gunSpawnPoints[Random.Range(0, gunSpawnPoints.Count)];

            // Instantioi ase
            GameObject gunInstance = Instantiate(weaponPairs[randomIndex].gun, gunSpawnPoint.position, gunSpawnPoint.rotation *  Quaternion.Euler(0,90,0));

            // Instantioi panokset ja liit‰ ne aseelle
            for (int j = 0; j < ammoToInstantiate; j++)
            {
                int randomAmmoSpawnIndex = GetRandomIndex(ammoSpawnPoints.Count, selectedAmmoSpawnIndices);
                Transform ammoSpawnPoint = ammoSpawnPoints[randomAmmoSpawnIndex];

                GameObject ammoInstance = Instantiate(weaponPairs[randomIndex].ammo, ammoSpawnPoint.position, ammoSpawnPoint.rotation);
                // Aseta panos-objekti aseen lapsiobjektiksi
                ammoInstance.transform.parent = gunInstance.transform;

                // Lis‰‰ valittu indeksi valittujen joukkoon
                selectedAmmoSpawnIndices.Add(randomAmmoSpawnIndex);
            }

            // Lis‰‰ valittu indeksi valittujen joukkoon
            selectedIndices.Add(randomIndex);
        }
    }

    int GetRandomIndex(int listLength, List<int> excludeIndices)
    {
        // Luo lista mahdollisista indekseist‰, pois lukien ne, jotka on jo valittu
        List<int> possibleIndices = new List<int>();
        for (int i = 0; i < listLength; i++)
        {
            if (!excludeIndices.Contains(i))
            {
                possibleIndices.Add(i);
            }
        }

        // Palauta satunnainen indeksi mahdollisten joukosta
        return possibleIndices[Random.Range(0, possibleIndices.Count)];
    }
}
