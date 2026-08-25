using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float lifeSpam = 10f;
    [SerializeField] private float bulletSpeed = 1.0f;

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

        lifeSpam -= Time.deltaTime;
        if(lifeSpam <= 0)
        {
            gameObject.SetActive(false);
            lifeSpam = 10f;
        }
    }

    private void OnCollisionEnter (Collision collision)
    {
        gameObject.SetActive(false);
        Debug.Log("Se Desactivo la bala");
    }
}
