using UnityEngine;

public class GameManager : MonoBehaviour 
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private PlayerController player;
    public PlayerController Player { get; private set; }

    private bool escPressed = false;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escPressed = !escPressed;
            Time.timeScale = escPressed ? 1.0f : 0.0f;
        }
    }

    public void RegisterPlayer(PlayerController p)
    {
        Player = p;
    }
}
