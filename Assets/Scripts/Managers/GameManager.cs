using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private PlayerController player;
    public PlayerController Player { get; private set; }

    private bool escPressed = false;

    [SerializeField] WaveController waveController;
    [SerializeField] GameObject itWinsCanvas;

    private void OnEnable()
    {
        itWinsCanvas.SetActive(false);
    }

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
        itWinsCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escPressed = !escPressed;
            Time.timeScale = escPressed ? 1.0f : 0.0f;
        }

        itWins();
    }

    public void RegisterPlayer(PlayerController p)
    {
        Player = p;
    }

    public void RetryLevel()
    {
        string thisScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(thisScene);
    }

    public void QuitLevel()
    {
        Application.Quit();
    }

    public void itWins()
    {
        if(waveController.waveCount == 5)
        {
            Time.timeScale = 0.0f;
            itWinsCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
