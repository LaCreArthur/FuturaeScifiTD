using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlinkFeedbacks : MonoBehaviour
{
    [SerializeField] float blinks;
    [SerializeField] Material blinkMaterial;

    public readonly Dictionary<MeshRenderer, Material[]> _rendererMaterial = new Dictionary<MeshRenderer, Material[]>();

    Coroutine _blinkCoroutine;
    HealthSystem _healthSystem;
    void Awake()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in renderers)
        {
            _rendererMaterial[meshRenderer] = meshRenderer.sharedMaterials;
        }
        _healthSystem = GetComponent<HealthSystem>();
        if (_healthSystem == null) _healthSystem = GetComponentInParent<HealthSystem>();
        if (_healthSystem == null)
        {
            Debug.LogWarning("No HealthSystem found in parent or self", this);
            enabled = false;
            return;
        }
        _healthSystem.DamageTaken += DamageTaken;
    }


    void OnDisable()
    {
        if (_blinkCoroutine != null) StopCoroutine(_blinkCoroutine);
        if (_rendererMaterial.Count > 0)
        {
            SetMaterial(false);
        }
    }

    void DamageTaken()
    {
        if (_healthSystem.CurrentHp <= 0) return;
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);
        _blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        for (int i = 0; i < blinks; i++)
        {
            SetMaterial(true);
            yield return new WaitForSeconds(0.05f);
            SetMaterial(false);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void SetMaterial(bool blink)
    {
        foreach (MeshRenderer r in _rendererMaterial.Keys)
        {
            r.sharedMaterials = blink ? new[] { blinkMaterial } : _rendererMaterial[r];
        }
    }
}
