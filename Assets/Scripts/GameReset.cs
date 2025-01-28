using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour
{
    public SpawnManager spawnManager; // SpawnManager referansý

    public void OnClick()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        // Sahnedeki tüm Spawnable nesneleri sil
        GameObject[] spawnables = GameObject.FindGameObjectsWithTag("Spawnable");
        foreach (GameObject obj in spawnables)
        {
            Destroy(obj); // Nesneyi sahneden kaldýr
        }

        // SpawnManager'ý kullanarak tüm nesneleri yeniden oluþtur
        if (spawnManager != null)
        {
            //spawnManager.spawnedPositions.Clear(); // Pozisyon listesini temizle
            spawnManager.SpawnAllObjects();       // Nesneleri yeniden oluþtur
        }
        else
        {
            Debug.LogError("SpawnManager referansý bulunamadý!");
        }

        // Skoru sýfýrla (PlacementController'dan baðlanabilir)
        var placementController = FindObjectOfType<PlacementController>();
        if (placementController != null)
        {
            placementController.UpdateScore(-placementController.score);
        }

        Debug.Log("Oyun sýfýrlandý!");
    }
}
