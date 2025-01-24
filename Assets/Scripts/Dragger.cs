using UnityEngine;

public class Dragger : MonoBehaviour
{

    private new ParticleSystem particleSystem; // Nesne üzerindeki Particle System
    public bool isDragging = false; // Sürükleme durumunu takip eder
    public Vector3 originalPosition; // Nesnenin baþlangýç pozisyonu
    private float liftHeight = 5f; // Nesnenin kalkacaðý yükseklik

    void Start()
    {
        originalPosition = transform.position;
        // Nesne üzerindeki Particle System'i al
        particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            particleSystem.Stop(); // Baþlangýçta partikülleri kapalý tut
        }
    }

    void OnMouseDrag()
    {
        // Sürükleme baþladýðýnda Particle System'i çalýþtýr
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
        // Sürükleme bittiðinde GameManager'a bildir
        if (particleSystem != null && isDragging)
        {
            particleSystem.Stop();
            isDragging = false;
        }
        GameObject placementArea = GameObject.Find("PlacementArea"); // PlacementArea referansý al
        Collider placementCollider = placementArea.GetComponent<Collider>();


    }
}