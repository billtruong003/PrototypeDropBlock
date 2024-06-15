using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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

            mySequence.Append(target.DOScale(new Vector3(1f, 0.7f, 1), duration * 0.2f).SetEase(Ease.OutQuad));
            mySequence.Append(target.DOScale(new Vector3(1, 1.1f, 1), duration * 0.2f).SetEase(Ease.OutQuad));
            mySequence.Append(target.DOScale(Vector3.one, duration * 0.2f).SetEase(Ease.OutQuad));

            // Play the sequence
            mySequence.Play();
        }


        /// <summary>
        /// Creates a press effect on the target Button without invoking the onClick event.
        /// </summary>
        /// <param name="button">The Button to apply the press effect.</param>
        /// <param name="pressDuration">The duration of the press effect.</param>
        /// <param name="pressColor">The color to use for the press effect.</param>
        public static void PressButton(Button button, float pressDuration = 0.1f)
        {
            if (button == null)
            {
                Debug.LogWarning("Button is null.");
                return;
            }

            // Get the RectTransform component of the Button
            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            if (buttonTransform == null)
            {
                Debug.LogWarning("Button does not have a RectTransform component.");
                return;
            }

            // Store the original scale of the Button
            Vector3 originalScale = buttonTransform.localScale;

            // Create a sequence for the press effect
            Sequence pressSequence = DOTween.Sequence();

            pressSequence.Append(buttonTransform.DOScale(originalScale * 0.9f, pressDuration).SetEase(Ease.OutQuad));
            pressSequence.Append(buttonTransform.DOScale(originalScale, pressDuration).SetEase(Ease.OutQuad));

            // Play the sequence
            pressSequence.Play();
        }

        public static void DOTriggerExplosion(List<GameObject> objectsToScatter, Vector3 explosionCenter, float explosionForce = 5f, float scatterDuration = 1f, float returnDuration = 0.5f)
        {
            if (objectsToScatter == null || objectsToScatter.Count == 0)
            {
                Debug.LogWarning("No objects provided for explosion effect.");
                return;
            }

            List<Vector3> originalPositions = new List<Vector3>();

            foreach (GameObject obj in objectsToScatter)
            {
                if (obj != null)
                {
                    originalPositions.Add(obj.transform.position);
                }
                else
                {
                    originalPositions.Add(Vector3.zero);
                }
            }

            for (int i = 0; i < objectsToScatter.Count; i++)
            {
                GameObject obj = objectsToScatter[i];
                if (obj == null) continue;

                Vector3 direction = (obj.transform.position - explosionCenter).normalized;
                Vector3 scatterPosition = obj.transform.position + direction * explosionForce;

                obj.transform.DOMove(scatterPosition, scatterDuration).SetEase(Ease.OutQuad);
            }

            DOVirtual.DelayedCall(scatterDuration, () =>
            {
                for (int i = 0; i < objectsToScatter.Count; i++)
                {
                    GameObject obj = objectsToScatter[i];
                    if (obj == null) continue;

                    obj.transform.DOMove(originalPositions[i], returnDuration).SetEase(Ease.InQuad).OnComplete(() =>
                    {
                        BlockController blockController = obj.transform.parent.GetComponentInParent<BlockController>();
                    });
                }
            });
        }
    }
}
