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
            GameObject bullets = Instantiate(bullet);
            bullets.SetActive(false);
            bulletList.Add(bullets);
            bullets.transform.parent = transform;
        }
    }

    public GameObject GetBullet(Transform transform)
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (!bulletList[i].activeSelf)
            {
                bulletList[i].transform.position = transform.position;
                bulletList[i].transform.rotation = transform.rotation;
                bulletList[i].SetActive(true);
                return bulletList[i];
            }
        }
        return null;
    }
}