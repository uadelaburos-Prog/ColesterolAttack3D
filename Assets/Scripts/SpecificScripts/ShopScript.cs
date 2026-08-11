using UnityEngine;
using TMPro;

public class ShopScript : MonoBehaviour
{
    [SerializeField] private CameraScript cam;
    [SerializeField] private WeaponScript weapon;
    private bool isPlayerInShop = false;
    private bool camaraLock = false;
    private void Update() 
    {
        if (isPlayerInShop)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                camaraLock = !camaraLock;
                Debug.Log("el player abrio la tienda");
                Cursor.lockState = camaraLock ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = camaraLock;
                cam.canMove = !camaraLock;
            }  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El player esta cerca de la tienda");
            isPlayerInShop = true;
            weapon.canShoot = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El player esta lejos de la tienda");
            isPlayerInShop = false;
            weapon.canShoot = true;
        }
    }
}
