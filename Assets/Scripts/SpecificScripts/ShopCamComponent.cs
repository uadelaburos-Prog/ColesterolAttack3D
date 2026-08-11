using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ShopCamComponent : MonoBehaviour
{
    [SerializeField] private LayerMask items;
    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f, items))
        {
            GameObject hitObject = hit.collider.gameObject;

            string name = hitObject.name;
            Debug.Log($"El raycast golpeo {name}");

            if (Input.GetKeyDown(KeyCode.E))
            {
                hitObject.SetActive(false);
            }
        }
    }

    //public void ApplyMaterial()
    //{
       
    //}
}