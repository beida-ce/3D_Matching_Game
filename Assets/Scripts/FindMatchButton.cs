using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatchButton : MonoBehaviour
{
    public Material highlightMaterial; // Rastgele se�ilen nesnelere ge�ici olarak uygulanacak materyal
    public float duration = 2f; // Materyalin uygulanma s�resi

    public void ApplyHighlightToRandomObjects()
    {
        // T�m "spawnable" etiketine sahip nesneleri al
        GameObject[] allSpawnableObjects = GameObject.FindGameObjectsWithTag("Spawnable");

        // Prefablara g�re grupland�r
        Dictionary<string, List<GameObject>> prefabGroups = new Dictionary<string, List<GameObject>>();

        foreach (GameObject obj in allSpawnableObjects)
        {
            // Prefab ismini al (unique bir belirte� olarak prefab ad�n� kullan�yoruz)
            string prefabName = obj.name.Replace("(Clone)", "").Trim();

            // E�er prefab grupta yoksa ekle
            if (!prefabGroups.ContainsKey(prefabName))
            {
                prefabGroups[prefabName] = new List<GameObject>();
            }

            // Obje gruba eklenir
            prefabGroups[prefabName].Add(obj);
        }

        // Uygun bir grup bulunup se�ilecek
        List<GameObject> selectedGroup = null;

        foreach (var group in prefabGroups.Values)
        {
            if (group.Count >= 2) // En az 2 nesneye sahip gruplar se�ilebilir
            {
                selectedGroup = group;
                break; // �lk uygun grubu al�yoruz (istenirse rastgele se�im yap�labilir)
            }
        }

        // Uygun bir grup bulunamazsa i�lemi sonland�r
        if (selectedGroup == null || selectedGroup.Count < 2)
        {
            Debug.LogWarning("Yeterli nesne bulunamad�.");
            return;
        }

        // Ayn� gruptan rastgele 2 farkl� nesne se�
        GameObject obj1 = selectedGroup[Random.Range(0, selectedGroup.Count)];
        GameObject obj2 = selectedGroup[Random.Range(0, selectedGroup.Count)];

        while (obj1 == obj2) // �kinci nesne birinciyle ayn�ysa yeni bir se�im yap
        {
            obj2 = selectedGroup[Random.Range(0, selectedGroup.Count)];
        }

        // Materyali ge�ici olarak uygula
        StartCoroutine(ApplyMaterialTemporarily(obj1, obj2));
    }

    private IEnumerator ApplyMaterialTemporarily(GameObject obj1, GameObject obj2)
    {
        // Objelerin mevcut materyallerini sakla
        MeshRenderer renderer1 = obj1.GetComponent<MeshRenderer>();
        MeshRenderer renderer2 = obj2.GetComponent<MeshRenderer>();

        Material[] originalMaterials1 = renderer1.materials;
        Material[] originalMaterials2 = renderer2.materials;

        // Highlight materyalini uygula
        renderer1.material = highlightMaterial;
        renderer2.material = highlightMaterial;

        // Belirtilen s�re kadar bekle
        yield return new WaitForSeconds(duration);

        // Eski materyalleri geri y�kle
        renderer1.materials = originalMaterials1;
        renderer2.materials = originalMaterials2;
    }
}
