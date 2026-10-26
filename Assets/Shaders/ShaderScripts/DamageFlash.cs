using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private MeshRenderer mR;
    private Color origColor;
    [SerializeField] private float flashTime = 0.15f;

    private void OnEnable()
    {
        mR = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        origColor = mR.material.color;
    }

    public void StartFlash()
    {
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        mR.material.color = Color.white;
        yield return new WaitForSeconds(flashTime);
        mR.material.color = origColor;
    }
}
