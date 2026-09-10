using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private float reduceSpeed = 2;
    private float target = 1;
    private Camera cam;


    private void Start()
    {
        cam = Camera.main;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        target  = currentHealth / maxHealth;
    }
    
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
}