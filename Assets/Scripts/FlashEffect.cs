using System.Collections;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] float duration = 0.125f;

    SpriteRenderer sr;
    Material originalMaterial;
    Coroutine flashRoutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        sr.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        sr.material = originalMaterial;

        flashRoutine = null;
    }
}
