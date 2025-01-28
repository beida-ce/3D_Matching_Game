using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatchButton : MonoBehaviour
{
    public Material highlightMaterial; // Rastgele seçilen nesnelere geçici olarak uygulanacak materyal
    public float duration = 2f; // Materyalin uygulanma süresi

    public void ApplyHighlightToRandomObjects()
    {
        // Tüm "spawnable" etiketine sahip nesneleri al
        GameObject[] allSpawnableObjects = GameObject.FindGameObjectsWithTag("Spawnable");

        // Prefablara göre gruplandýr
        Dictionary<string, List<GameObject>> prefabGroups = new Dictionary<string, List<GameObject>>();

        foreach (GameObject obj in allSpawnableObjects)
        {
            // Prefab ismini al (unique bir belirteç olarak prefab adýný kullanýyoruz)
            string prefabName = obj.name.Replace("(Clone)", "").Trim();

            // Eðer prefab grupta yoksa ekle
            if (!prefabGroups.ContainsKey(prefabName))
            {
                prefabGroups[prefabName] = new List<GameObject>();
            }

            // Obje gruba eklenir
            prefabGroups[prefabName].Add(obj);
        }

        // Uygun bir grup bulunup seçilecek
        List<GameObject> selectedGroup = null;

        foreach (var group in prefabGroups.Values)
        {
            if (group.Count >= 2) // En az 2 nesneye sahip gruplar seçilebilir
            {
                selectedGroup = group;
                break; // Ýlk uygun grubu alýyoruz (istenirse rastgele seçim yapýlabilir)
            }
        }

        // Uygun bir grup bulunamazsa iþlemi sonlandýr
        if (selectedGroup == null || selectedGroup.Count < 2)
        {
            Debug.LogWarning("Yeterli nesne bulunamadý.");
            return;
        }

        // Ayný gruptan rastgele 2 farklý nesne seç
        GameObject obj1 = selectedGroup[Random.Range(0, selectedGroup.Count)];
        GameObject obj2 = selectedGroup[Random.Range(0, selectedGroup.Count)];

        while (obj1 == obj2) // Ýkinci nesne birinciyle aynýysa yeni bir seçim yap
        {
            obj2 = selectedGroup[Random.Range(0, selectedGroup.Count)];
        }

        // Materyali geçici olarak uygula
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

        // Belirtilen süre kadar bekle
        yield return new WaitForSeconds(duration);

        // Eski materyalleri geri yükle
        renderer1.materials = originalMaterials1;
        renderer2.materials = originalMaterials2;
    }
}
