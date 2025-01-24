using UnityEngine;

public class Dragger : MonoBehaviour
{

    private new ParticleSystem particleSystem; // Nesne �zerindeki Particle System
    public bool isDragging = false; // S�r�kleme durumunu takip eder
    public Vector3 originalPosition; // Nesnenin ba�lang�� pozisyonu
    private float liftHeight = 5f; // Nesnenin kalkaca�� y�kseklik

    void Start()
    {
        originalPosition = transform.position;
        // Nesne �zerindeki Particle System'i al
        particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            particleSystem.Stop(); // Ba�lang��ta partik�lleri kapal� tut
        }
    }

    void OnMouseDrag()
    {
        // S�r�kleme ba�lad���nda Particle System'i �al��t�r
        if (particleSystem != null && !isDragging)
        {
            particleSystem.Play();
            isDragging = true;
        }

        // Fare pozisyonunu takip ederek nesneyi hareket ettir
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;
    }


    void OnMouseUp()
    {
        // S�r�kleme bitti�inde GameManager'a bildir
        if (particleSystem != null && isDragging)
        {
            particleSystem.Stop();
            isDragging = false;
        }
        GameObject placementArea = GameObject.Find("PlacementArea"); // PlacementArea referans� al
        Collider placementCollider = placementArea.GetComponent<Collider>();


    }
}