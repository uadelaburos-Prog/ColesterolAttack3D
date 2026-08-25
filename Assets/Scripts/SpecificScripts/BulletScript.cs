using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float lifeSpam = 10f;
    [SerializeField] private float bulletSpeed = 1.0f;
    [SerializeField] private Enemy Enemy;

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

        lifeSpam -= Time.deltaTime;
        if(lifeSpam <= 0)
        {
            gameObject.SetActive(false);
            lifeSpam = 5f;
        }
    }

    private void OnCollisionEnter (Collision collision)
    {
        gameObject.SetActive(false);
        Enemy = collision.gameObject.GetComponent<Enemy>();
        Debug.Log($"Obtubo el componente Enemy de: {Enemy.gameObject.name}");
        Enemy.ReciveDmg(true);
    }
}
