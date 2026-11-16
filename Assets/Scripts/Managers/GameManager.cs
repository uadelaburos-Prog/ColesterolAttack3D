using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController player;
    public PlayerController Player { get; private set; }

    private bool escPressed = false;

    [SerializeField] WaveController waveController;
    [SerializeField] GameObject itWinsCanvas;

    public CheckpointSave checkpointSave { get; } = new CheckpointSave();

    private void OnEnable()
    {
        itWinsCanvas.SetActive(false);
        waveController = FindAnyObjectByType<WaveController>();
    }

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<GameManager>();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public void RegisterPlayer(PlayerController player)
    {
        Player = player;
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

    [System.Obsolete]
    public void RetryLevel()
    {
        Time.timeScale = 0.0f;
        string thisScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(thisScene);
        itWinsCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitLevel()
    {
        Application.Quit();
    }

    public void itWins()
    {
        if(waveController.waveCount == 5)
        {
            checkpointSave.Clear();
            Time.timeScale = 0.0f;
            itWinsCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            waveController.waveCount = 1;
        }
    }
}
