using System.Collections.Generic;
using UnityEngine;

public class GunTest2 : MonoBehaviour
{
    public List<GameObject> zombiesToActivate = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateZombies();
        }
    }

    private void ActivateZombies()
    {
        foreach (GameObject zombie in zombiesToActivate)
        {
            zombie.SetActive(true);
        }
    }
}
