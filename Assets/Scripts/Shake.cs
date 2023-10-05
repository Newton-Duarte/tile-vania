using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] Vector2 minShakeForce = new Vector2(-0.05f, -0.05f);
    [SerializeField] Vector2 maxShakeForce = new Vector2(0.05f, 0.05f);
    [SerializeField] float shakeInterval = 0.01f;

    public bool isShaking = false;

    Coroutine shakeRoutine;
    Vector3 initialPosition;


    void Start()
    {
        initialPosition = transform.position;
    }

    public void StopShaking()
    {
        isShaking = false;
        StopCoroutine(shakeRoutine);
    }

    public void StartShaking()
    {
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }

        shakeRoutine = StartCoroutine(ShakeRoutine());
        isShaking = true;
    }

    IEnumerator ShakeRoutine()
    {
        while (true)
        {
            float offsetX = UnityEngine.Random.Range(minShakeForce.x, maxShakeForce.x);
            float offsetY = UnityEngine.Random.Range(minShakeForce.y, maxShakeForce.y);

            transform.position = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);

            yield return new WaitForSeconds(shakeInterval);

            transform.position = initialPosition;
        }
    }
}
