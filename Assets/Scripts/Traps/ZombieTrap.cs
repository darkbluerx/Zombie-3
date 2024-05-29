using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTrap : MonoBehaviour
{
    [SerializeField] List<GameObject> zombiesToActivate = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateZombies();
        }
    }

    private void ActivateZombies()
    {
        if (zombiesToActivate.Count == 0) return;

        foreach (GameObject zombie in zombiesToActivate)
        {
            if (zombie != null)
            {
                zombie.SetActive(true);
            }     
        }
    }
}
