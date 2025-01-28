using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour
{
    public SpawnManager spawnManager; // SpawnManager referans�

    public void OnClick()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        // Sahnedeki t�m Spawnable nesneleri sil
        GameObject[] spawnables = GameObject.FindGameObjectsWithTag("Spawnable");
        foreach (GameObject obj in spawnables)
        {
            Destroy(obj); // Nesneyi sahneden kald�r
        }

        // SpawnManager'� kullanarak t�m nesneleri yeniden olu�tur
        if (spawnManager != null)
        {
            //spawnManager.spawnedPositions.Clear(); // Pozisyon listesini temizle
            spawnManager.SpawnAllObjects();       // Nesneleri yeniden olu�tur
        }
        else
        {
            Debug.LogError("SpawnManager referans� bulunamad�!");
        }

        // Skoru s�f�rla (PlacementController'dan ba�lanabilir)
        var placementController = FindObjectOfType<PlacementController>();
        if (placementController != null)
        {
            placementController.UpdateScore(-placementController.score);
        }

        Debug.Log("Oyun s�f�rland�!");
    }
}
