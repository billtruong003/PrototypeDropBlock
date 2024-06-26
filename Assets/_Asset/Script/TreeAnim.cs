using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Collections;

public class TreeWindEffect : MonoBehaviour
{
    public float shakeDuration = 1.5f;
    public float strength = 0.05f;
    public int vibrato = 10;
    public float randomness = 10;
    public bool fadeOut = true;
    public float delayBetweenShakes = 2.0f;

    void Start()
    {
        StartCoroutine(StartWindEffect());
    }

    private IEnumerator StartWindEffect()
    {
        while (true)
        {
            transform.DOShakeScale(shakeDuration, strength, vibrato, randomness, fadeOut)
                .SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(shakeDuration + delayBetweenShakes);
        }
    }
}
