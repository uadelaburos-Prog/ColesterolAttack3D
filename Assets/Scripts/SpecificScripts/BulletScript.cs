using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float lifeSpam = 10f;
    [SerializeField] private float bulletSpeed = 1.0f;

    void Update()
    {
        transform.Translate(Vector3.back * bulletSpeed * Time.deltaTime);

        lifeSpam -= Time.deltaTime;
        if(lifeSpam <= 0)
        {
            Destroy(gameObject);
            lifeSpam = 10f;
        }
    }

    private void OnCollisionEnter (Collision collision)
    {
        Destroy(gameObject);
        Debug.Log("Se destruyo la bala");
    }
}
