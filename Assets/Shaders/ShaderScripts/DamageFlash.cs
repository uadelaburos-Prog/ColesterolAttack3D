using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private float flashTime = 0.15f;

    private MeshRenderer mR;
    private Color origColor;
    private bool colorSaved;
    private Coroutine flashRoutine;

    private void Awake()
    {
        mR = GetComponent<MeshRenderer>();
        if (mR != null)
        {
            origColor = mR.material.color;
            colorSaved = true;
        }
    }

    private void OnDisable()
    {
        flashRoutine = null;
        if (mR != null && colorSaved) mR.material.color = origColor;
    }

    public void StartFlash()
    {
        if (mR == null || !isActiveAndEnabled) return;

        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        mR.material.color = Color.white;
        yield return new WaitForSeconds(flashTime);
        mR.material.color = origColor;
        flashRoutine = null;
    }
}
