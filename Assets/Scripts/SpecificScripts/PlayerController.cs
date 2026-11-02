using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    private PlayerStats stats;

    [SerializeField] private bool useGetAxisRaw;

    [SerializeField] private Transform cameraPlayer;

    [SerializeField] private int maxLife = 3;
    [SerializeField] private int currentLife;

    [Header("PlayerDie")]
    [SerializeField] private GameObject playerCanvas;
    private bool itDie;

    private CharacterController controller;
    private float _gravity = 9.81f;
    [SerializeField] private float gravityMult;
    private float velocity;
    private Vector3 diretion;

    private Rigidbody rb;

    public PlayerStats Stats => stats;

    private void Awake()
    {
        currentLife = maxLife;

        controller = GetComponent<CharacterController>();

        stats = FindAnyObjectByType<PlayerStats>();

        if(cameraPlayer == null && Camera.main != null)
        {
            cameraPlayer = Camera.main.transform;
        }  

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
        playerCanvas.SetActive(false);
    }

    void Update()
    {
        //movimiento
        float xMove = useGetAxisRaw ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        float yMove = useGetAxisRaw ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

        Vector3 fowardCamera = cameraPlayer.forward;
        Vector3 rightCamera = cameraPlayer.right;

        fowardCamera.y = 0f;
        rightCamera.y = 0f;

        fowardCamera.Normalize();
        rightCamera.Normalize();

        diretion = (rightCamera * xMove + fowardCamera * yMove);

        if(diretion.sqrMagnitude > 0.0001f)
            diretion.Normalize();

        Vector3 movement = diretion * (stats.playerSpeed * Time.deltaTime);

        controller.Move(movement);

        //vida
        if (currentLife <= 0f)
        {
            StartCoroutine(playerDies());
        }
    }

    private void FixedUpdate()
    {
        if (controller.isGrounded && velocity < 0.0f) 
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += _gravity * gravityMult * Time.fixedDeltaTime;
            diretion.y = velocity;
        }
    }

    public void Life(int damage)
    {
        currentLife -= damage;
    }

    private IEnumerator playerDies()
    {
        Time.timeScale = 0f;
        playerCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Se murio");
    }
}
