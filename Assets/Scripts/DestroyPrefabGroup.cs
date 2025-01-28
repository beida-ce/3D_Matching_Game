using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefabGroup : MonoBehaviour
{
    public List<GameObject> prefabsToDestroy; // Kontrol edilecek prefablar listesi
    public AudioClip destroySound; // Yok etme ses efekti
    public float destructionDelay = 0.5f; // Yok etmeden �nceki bekleme s�resi
    public float soundVolume = 10.0f; // Sesin �iddeti

    public void DestroyRandomPrefabGroup()
    {
        if (prefabsToDestroy == null || prefabsToDestroy.Count == 0)
        {
            Debug.LogWarning("Hi� prefab atanmad�! L�tfen prefab listesini doldurun.");
            return;
        }

        // Sahnedeki "Spawnable" etiketli nesneleri bul
        GameObject[] spawnableObjects = GameObject.FindGameObjectsWithTag("Spawnable");

        if (spawnableObjects.Length == 0)
        {
            Debug.LogWarning("Hi� 'Spawnable' etiketine sahip nesne bulunamad�!");
            return;
        }

        // Prefablara g�re grupland�r
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

        // Sadece sahnede olan prefab gruplar�n� filtrele
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
            Debug.LogWarning("Ekranda e�le�en hi�bir prefab bulunamad�!");
            return;
        }

        // Rastgele bir prefab se�
        GameObject randomPrefab = availablePrefabs[Random.Range(0, availablePrefabs.Count)];
        List<GameObject> objectsToDestroy = groupedObjects[randomPrefab];

        // Se�ilen prefaba ait t�m nesneleri yok et
        StartCoroutine(DestroyObjectsWithSound(objectsToDestroy));
    }

    private IEnumerator DestroyObjectsWithSound(List<GameObject> objectsToDestroy)
    {
        // Ses efekti �al (iste�e ba�l�, sadece ilk nesne i�in �alar)
        if (destroySound != null && objectsToDestroy.Count > 0)
        {
            AudioSource.PlayClipAtPoint(destroySound, objectsToDestroy[0].transform.position, soundVolume);
        }

        // Yok etmeden �nce iste�e ba�l� bir bekleme s�resi
        yield return new WaitForSeconds(destructionDelay);

        // T�m nesneleri yok et
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        Debug.Log($"{objectsToDestroy.Count} adet nesne yok edildi.");
    }
}
