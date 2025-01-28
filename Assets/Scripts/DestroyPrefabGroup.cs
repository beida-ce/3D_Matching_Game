using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefabGroup : MonoBehaviour
{
    public List<GameObject> prefabsToDestroy; // Kontrol edilecek prefablar listesi
    public AudioClip destroySound; // Yok etme ses efekti
    public float destructionDelay = 0.5f; // Yok etmeden önceki bekleme süresi
    public float soundVolume = 10.0f; // Sesin þiddeti

    public void DestroyRandomPrefabGroup()
    {
        if (prefabsToDestroy == null || prefabsToDestroy.Count == 0)
        {
            Debug.LogWarning("Hiç prefab atanmadý! Lütfen prefab listesini doldurun.");
            return;
        }

        // Sahnedeki "Spawnable" etiketli nesneleri bul
        GameObject[] spawnableObjects = GameObject.FindGameObjectsWithTag("Spawnable");

        if (spawnableObjects.Length == 0)
        {
            Debug.LogWarning("Hiç 'Spawnable' etiketine sahip nesne bulunamadý!");
            return;
        }

        // Prefablara göre gruplandýr
        Dictionary<GameObject, List<GameObject>> groupedObjects = new Dictionary<GameObject, List<GameObject>>();

        foreach (GameObject prefab in prefabsToDestroy)
        {
            groupedObjects[prefab] = new List<GameObject>();
        }

        foreach (GameObject obj in spawnableObjects)
        {
            foreach (GameObject prefab in prefabsToDestroy)
            {
                if (obj.name.Replace("(Clone)", "").Trim() == prefab.name.Trim())
                {
                    groupedObjects[prefab].Add(obj);
                    break;
                }
            }
        }

        // Sadece sahnede olan prefab gruplarýný filtrele
        List<GameObject> availablePrefabs = new List<GameObject>();
        foreach (var kvp in groupedObjects)
        {
            if (kvp.Value.Count > 0)
            {
                availablePrefabs.Add(kvp.Key);
            }
        }

        if (availablePrefabs.Count == 0)
        {
            Debug.LogWarning("Ekranda eþleþen hiçbir prefab bulunamadý!");
            return;
        }

        // Rastgele bir prefab seç
        GameObject randomPrefab = availablePrefabs[Random.Range(0, availablePrefabs.Count)];
        List<GameObject> objectsToDestroy = groupedObjects[randomPrefab];

        // Seçilen prefaba ait tüm nesneleri yok et
        StartCoroutine(DestroyObjectsWithSound(objectsToDestroy));
    }

    private IEnumerator DestroyObjectsWithSound(List<GameObject> objectsToDestroy)
    {
        // Ses efekti çal (isteðe baðlý, sadece ilk nesne için çalar)
        if (destroySound != null && objectsToDestroy.Count > 0)
        {
            AudioSource.PlayClipAtPoint(destroySound, objectsToDestroy[0].transform.position, soundVolume);
        }

        // Yok etmeden önce isteðe baðlý bir bekleme süresi
        yield return new WaitForSeconds(destructionDelay);

        // Tüm nesneleri yok et
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        Debug.Log($"{objectsToDestroy.Count} adet nesne yok edildi.");
    }
}
