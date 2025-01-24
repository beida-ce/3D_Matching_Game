using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public int maxObjectsInArea = 2; // Placement alanýnda maksimum nesne sayýsý
    public AudioSource matchSound; // Eþleþme sesi
    public AudioSource mismatchSound; // Eþleþme baþarýsýz sesi
    public ParticleSystem matchParticles; // Eþleþme partikül efektleri
    public Transform resetPosition; // Baþarýsýz nesnelerin döneceði pozisyon
    public TMPro.TextMeshProUGUI scoreText; // Skor göstergesi (UI)

    public int score = 0; // Toplam skor
    private List<GameObject> objectsInPlacementArea = new List<GameObject>(); // Placement alanýndaki nesneler

    void Start()
    {
        UpdateScore(0); // Skoru sýfýrla ve göster
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawnable"))
        {
            var dragger = other.GetComponent<Dragger>();
            if (dragger != null && dragger.isDragging) // Sadece fare ile býrakýlmýþ nesneler
            {
                // Placement alanýna giren nesne
                if (objectsInPlacementArea.Count < maxObjectsInArea)
                {
                    if (!objectsInPlacementArea.Contains(other.gameObject))
                    {
                        objectsInPlacementArea.Add(other.gameObject);
                        Debug.Log(other.name + " placement alanýna eklendi.");

                        // Nesneyi sabitle
                        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                        // Eþleþme kontrolü yap
                        if (objectsInPlacementArea.Count == maxObjectsInArea)
                        {
                            CheckMatch();
                        }
                    }
                }
                else
                {
                    Debug.Log("Placement alaný dolu. " + other.name + " reddedildi.");
                    ResetObject(other.gameObject);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spawnable"))
        {
            objectsInPlacementArea.Remove(other.gameObject);
            Debug.Log(other.name + " placement alanýndan çýkarýldý.");
        }
    }

    public void CheckMatch()
    {
        if (objectsInPlacementArea.Count == maxObjectsInArea)
        {
            GameObject firstObject = objectsInPlacementArea[0];
            GameObject secondObject = objectsInPlacementArea[1];

            if (firstObject.name == secondObject.name) // Prefab adlarýna göre eþleþme kontrolü
            {
                Debug.Log("Eþleþme baþarýlý: " + firstObject.name);

                // Skoru güncelle
                UpdateScore(50);

                // Eþleþme sesi ve partikül efekti çalýþtýr
                if (matchSound != null) matchSound.Play();
                if (matchParticles != null)
                {
                    matchParticles.transform.position = firstObject.transform.position;
                    matchParticles.Play();
                }

                // Nesneleri yok et
                Destroy(firstObject);
                Destroy(secondObject);
            }
            else
            {
                Debug.Log("Eþleþme baþarýsýz!");
                UpdateScore(-25);

                // Baþarýsýz ses efekti
                if (mismatchSound != null) mismatchSound.Play();

                // Nesneleri resetle
                ResetObject(firstObject);
                ResetObject(secondObject);
            }

            // Placement alanýndaki nesne listesini temizle
            objectsInPlacementArea.Clear();
        }
    }

    private void ResetObject(GameObject obj)
    {
        if (obj != null)
        {
            var dragger = obj.GetComponent<Dragger>();
            if (dragger != null)
            {
                obj.transform.position = dragger.originalPosition; // Nesnenin orijinal pozisyonuna döndür
            }
            else if (resetPosition != null)
            {
                obj.transform.position = resetPosition.position; // Alternatif olarak sabit reset pozisyonu kullan
            }

            obj.GetComponent<Rigidbody>().isKinematic = false; // Nesneyi yeniden hareketli yap
            Debug.Log(obj.name + " orijinal pozisyonuna döndürüldü.");
        }
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        Debug.Log("Güncel skor: " + score);
    }
}