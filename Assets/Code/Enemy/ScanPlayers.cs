using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanPlayers : MonoBehaviour
{
    [SerializeField] GameObject findPlayer;

    public GameObject F_GetFindOBj()
    {
        return findPlayer;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && findPlayer == null)
        {
            findPlayer = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && findPlayer != null)
        {
            findPlayer = null;
        }
    }
}
