using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnablePrefab
{
    public GameObject prefab; // Spawn edilecek prefab
    public int count; // Üretilecek nesne sayýsý
}

public class SpawnManager : MonoBehaviour
{
    public List<SpawnablePrefab> prefabsToSpawn; // Spawn edilecek prefablar ve sayýlarý
    public Vector2 spawnAreaMin; // Spawn alanýnýn sol alt köþesi (X, Z)
    public Vector2 spawnAreaMax; // Spawn alanýnýn sað üst köþesi (X, Z)
    public float spawnHeight = 2f; // Spawn yüksekliði
    public float minDistance = 1f; // Nesneler arasýndaki minimum mesafe

    public AudioClip backgroundMusic; // Arka plan müzik dosyasý
    private AudioSource audioSource; // AudioSource bileþeni

    private List<GameObject> spawnedObjects = new List<GameObject>(); // Spawn edilen nesneler (bu listeyi kullanarak nesneleri takip edeceðiz)

    void Start()
    {
        // AudioSource bileþenini ayarla
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // Müzik döngüye alýnsýn
        audioSource.playOnAwake = false; // Otomatik çalmasýn
        audioSource.volume = 0.2f; // Ses seviyesini ayarla
        audioSource.Play(); // Müzik baþlasýn

        // Prefablarý spawn et
        SpawnAllObjects();
    }

    void Update()
    {
        // Sahnedeki nesneleri kontrol et ve eðer hiç nesne yoksa yeniden spawn et
        if (spawnedObjects.Count == 0)
        {
            SpawnAllObjects(); // Eðer hiç nesne yoksa yeniden spawn et
        }
        else
        {
            // Sahnedeki spawn edilen nesnelerin hala var olup olmadýðýný kontrol et
            for (int i = spawnedObjects.Count - 1; i >= 0; i--)
            {
                if (spawnedObjects[i] == null)
                {
                    spawnedObjects.RemoveAt(i); // Nesne yoksa listeden çýkar
                }
            }
        }
    }

    public void SpawnAllObjects()
    {
        foreach (var spawnable in prefabsToSpawn)
        {
            SpawnObjectsForPrefab(spawnable.prefab, spawnable.count);
        }
    }

    void SpawnObjectsForPrefab(GameObject prefab, int count)
    {
        int spawned = 0;
        int maxAttempts = 100; // Sonsuz döngüyü önlemek için bir sýnýr belirliyoruz

        while (spawned < count)
        {
            if (TrySpawnPrefab(prefab))
            {
                spawned++;
            }
            else
            {
                maxAttempts--;
                if (maxAttempts <= 0)
                {
                    Debug.LogWarning($"Maksimum deneme sayýsýna ulaþýldý. {prefab.name} için daha fazla nesne spawn edilemiyor.");
                    break;
                }
            }
        }
    }

    bool TrySpawnPrefab(GameObject prefab)
    {
        // Rastgele bir X-Z konumu seç
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            spawnHeight, // Yükseklik
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Pozisyon uygun mu kontrol et
        if (IsPositionValid(randomPosition))
        {
            GameObject newObject = Instantiate(prefab, randomPosition, Quaternion.identity);
            spawnedObjects.Add(newObject); // Spawn edilen nesneyi listeye ekle
            return true;
        }

        return false;
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (GameObject spawnedObject in spawnedObjects)
        {
            // Daha önceki pozisyonlarla mesafeyi kontrol et
            if (Vector3.Distance(position, spawnedObject.transform.position) < minDistance)
            {
                return false; // Pozisyon çok yakýn, geçersiz
            }
        }

        return true; // Pozisyon uygun
    }
}
