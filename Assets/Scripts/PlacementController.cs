using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public int maxObjectsInArea = 2; // Placement alan�nda maksimum nesne say�s�
    public AudioSource matchSound; // E�le�me sesi
    public AudioSource mismatchSound; // E�le�me ba�ar�s�z sesi
    public ParticleSystem matchParticles; // E�le�me partik�l efektleri
    public Transform resetPosition; // Ba�ar�s�z nesnelerin d�nece�i pozisyon
    public TMPro.TextMeshProUGUI scoreText; // Skor g�stergesi (UI)

    public int score = 0; // Toplam skor
    private List<GameObject> objectsInPlacementArea = new List<GameObject>(); // Placement alan�ndaki nesneler

    void Start()
    {
        UpdateScore(0); // Skoru s�f�rla ve g�ster
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawnable"))
        {
            var dragger = other.GetComponent<Dragger>();
            if (dragger != null && dragger.isDragging) // Sadece fare ile b�rak�lm�� nesneler
            {
                // Placement alan�na giren nesne
                if (objectsInPlacementArea.Count < maxObjectsInArea)
                {
                    if (!objectsInPlacementArea.Contains(other.gameObject))
                    {
                        objectsInPlacementArea.Add(other.gameObject);
                        Debug.Log(other.name + " placement alan�na eklendi.");

                        // Nesneyi sabitle
                        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                        // E�le�me kontrol� yap
                        if (objectsInPlacementArea.Count == maxObjectsInArea)
                        {
                            CheckMatch();
                        }
                    }
                }
                else
                {
                    Debug.Log("Placement alan� dolu. " + other.name + " reddedildi.");
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
            Debug.Log(other.name + " placement alan�ndan ��kar�ld�.");
        }
    }

    public void CheckMatch()
    {
        if (objectsInPlacementArea.Count == maxObjectsInArea)
        {
            GameObject firstObject = objectsInPlacementArea[0];
            GameObject secondObject = objectsInPlacementArea[1];

            if (firstObject.name == secondObject.name) // Prefab adlar�na g�re e�le�me kontrol�
            {
                Debug.Log("E�le�me ba�ar�l�: " + firstObject.name);

                // Skoru g�ncelle
                UpdateScore(50);

                // E�le�me sesi ve partik�l efekti �al��t�r
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
                Debug.Log("E�le�me ba�ar�s�z!");
                UpdateScore(-25);

                // Ba�ar�s�z ses efekti
                if (mismatchSound != null) mismatchSound.Play();

                // Nesneleri resetle
                ResetObject(firstObject);
                ResetObject(secondObject);
            }

            // Placement alan�ndaki nesne listesini temizle
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
                obj.transform.position = dragger.originalPosition; // Nesnenin orijinal pozisyonuna d�nd�r
            }
            else if (resetPosition != null)
            {
                obj.transform.position = resetPosition.position; // Alternatif olarak sabit reset pozisyonu kullan
            }

            obj.GetComponent<Rigidbody>().isKinematic = false; // Nesneyi yeniden hareketli yap
            Debug.Log(obj.name + " orijinal pozisyonuna d�nd�r�ld�.");
        }
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        Debug.Log("G�ncel skor: " + score);
    }
}