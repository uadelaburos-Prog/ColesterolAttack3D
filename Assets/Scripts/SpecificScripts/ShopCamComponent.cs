using UnityEngine;

public class ShopCamComponent : MonoBehaviour
{
    [SerializeField] private LayerMask items;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private StatOrdener StatsOrder;
    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f, items))
        {
            GameObject hitObject = hit.collider.gameObject;
            var instance = hitObject.GetComponent<ItemSourceData>();
            if (instance == null) return;

            string ID = instance.itemID;

            if (Input.GetKeyDown(KeyCode.E))
            {
                bool purch = shopManager.TryToPurchItem(instance.sourceData.ItemID);
                if (purch)
                {
                    Debug.Log($"Se compro el item {ID}");
                    hitObject.SetActive(false);
                    StatsOrder.ShowStats();
                }
                else
                    Debug.Log($"No se pudo comprar {ID} (sin fondos?)");

            }
        }
    }
}