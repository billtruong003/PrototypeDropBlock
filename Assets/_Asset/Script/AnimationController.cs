using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using DG.Tweening;
using UnityEngine;

namespace AnimationController.WithTransform
{
    public static class Anim
    {
        /// <summary>
        /// Creates a jelly bounce effect on the target transform.
        /// </summary>
        /// <param name="target">The transform to apply the jelly bounce effect.</param>
        /// <param name="duration">The total duration of the jelly bounce effect.</param>
        public static void JellyBounce(Transform target, float duration = 0.5f)
        {
            // Ensure the duration is greater than zero to avoid division by zero
            if (duration <= 0)
            {
                Debug.LogWarning("Duration must be greater than zero.");
                return;
            }

            // Create a sequence for the jelly bounce effect
            Sequence mySequence = DOTween.Sequence();

            // Add scaling animations to the sequence
            // mySequence.Append(target.DOScale(new Vector3(1.2f, 0.8f, 1), duration * 0.2f).SetEase(Ease.OutQuad));
            // mySequence.Append(target.DOScale(new Vector3(0.8f, 1.2f, 1), duration * 0.2f).SetEase(Ease.OutQuad));
            // mySequence.Append(target.DOScale(new Vector3(1.1f, 0.9f, 1), duration * 0.2f).SetEase(Ease.OutQuad));
            // mySequence.Append(target.DOScale(new Vector3(0.9f, 1.1f, 1), duration * 0.2f).SetEase(Ease.OutQuad));
            mySequence.Append(target.DOScale(new Vector3(1f, 0.7f, 1), duration * 0.2f).SetEase(Ease.OutQuad));
            mySequence.Append(target.DOScale(new Vector3(1.1f, 1.1f, 1), duration * 0.2f).SetEase(Ease.OutQuad));
            mySequence.Append(target.DOScale(Vector3.one, duration * 0.2f).SetEase(Ease.OutQuad));

            // Play the sequence
            mySequence.Play();
        }
    }
}
