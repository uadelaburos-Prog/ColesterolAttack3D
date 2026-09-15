using ED262C;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    [SerializeField] private int poolSize = 10;

    public SimpleArrayList<GameObject> bulletList = new SimpleArrayList<GameObject>();

    private static BulletPool instance;
    public static BulletPool Instance { get { return instance; } }

    private void Awake() // Singletone: solo puede existir uno
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        AddBulletsToPool(poolSize);
    }

    private void AddBulletsToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CreateBullet();
        }
    }

    private GameObject CreateBullet()
    {
        GameObject newBullet = Instantiate(bullet);
        newBullet.SetActive(false);
        newBullet.transform.SetParent(transform, false);
        bulletList.Add(newBullet);
        return newBullet;
    }

    public GameObject GetBullet(Transform spawnPoint)
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (!bulletList[i].activeSelf)
            {
                return ActivateBullet(bulletList[i], spawnPoint);
            }
        }

        // No había ninguna libre: el pool crece
        GameObject newBullet = CreateBullet();
        return ActivateBullet(newBullet, spawnPoint);
    }

    private GameObject ActivateBullet(GameObject bulletObj, Transform spawnPoint)
    {
        bulletObj.transform.position = spawnPoint.position;
        bulletObj.transform.rotation = spawnPoint.rotation;
        bulletObj.SetActive(true);
        return bulletObj;
    }
}