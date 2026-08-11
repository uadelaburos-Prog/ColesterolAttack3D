using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    private PlayerStats stats;

    [SerializeField] private bool useGetAxisRaw;

    [SerializeField] private Transform cameraPlayer;

    private CharacterController controller;

    public PlayerStats Stats => stats;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        stats = FindAnyObjectByType<PlayerStats>();

        if(cameraPlayer == null && Camera.main != null)
        {
            cameraPlayer = Camera.main.transform;
        }  
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
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

        Vector3 direction = (rightCamera * xMove + fowardCamera * yMove);

        if(direction.sqrMagnitude > 0.0001f) 
            direction.Normalize();

        Vector3 movement = direction * (stats.playerSpeed * Time.deltaTime);

        controller.Move(movement);
    }
}
