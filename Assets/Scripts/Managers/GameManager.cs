using UnityEngine;

public class GameManager : MonoBehaviour 
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private PlayerController player;
    public PlayerController Player { get; private set; }

    void Awake()
    {
        if(instance == null) { instance = this; }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        player = FindAnyObjectByType<PlayerController>();
    }

    private void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void RegisterPlayer(PlayerController p)
    {
        Player = p;
    }
}
