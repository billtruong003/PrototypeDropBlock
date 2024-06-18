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

            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            if (buttonTransform == null)
            {
                Debug.LogWarning("Button does not have a RectTransform component.");
                return;
            }
            buttonTransform.localScale = Vector3.one;
            Vector3 originalScale = buttonTransform.localScale;

            Sequence pressSequence = DOTween.Sequence();

            pressSequence.Append(buttonTransform.DOScale(originalScale * 0.9f, pressDuration).SetEase(Ease.OutQuad));
            pressSequence.Append(buttonTransform.DOScale(originalScale, pressDuration).SetEase(Ease.OutQuad));

            pressSequence.Play();
        }

        public static void DOTriggerExplosion(List<GameObject> objectsToScatter, Vector3 explosionCenter, float explosionForce = 10f, float scatterDuration = 0.3f, float returnDuration = 0)
        {
            if (objectsToScatter == null || objectsToScatter.Count == 0)
            {
                Debug.LogWarning("No objects provided for explosion effect.");
                return;
            }

            List<Vector3> originalPositions = new List<Vector3>();
            Sequence sequence = DOTween.Sequence();

            foreach (GameObject obj in objectsToScatter)
            {
                if (obj != null)
                {
                    originalPositions.Add(obj.transform.position);

                    Vector3 direction = (obj.transform.position - explosionCenter).normalized;
                    Vector3 scatterPosition = obj.transform.position + direction * Random.Range(explosionForce / 2f, explosionForce);

                    sequence.Join(obj.transform.DOMove(scatterPosition, scatterDuration).SetEase(Ease.OutQuad));
                }
                else
                {
                    originalPositions.Add(Vector3.zero);
                }
            }

            sequence.AppendInterval(scatterDuration);


            sequence.OnComplete(() =>
            {
                BlockController blockController = objectsToScatter[0].transform.parent.GetComponentInParent<BlockController>();
                OnExplosionComplete(blockController);
                for (int i = 0; i < objectsToScatter.Count; i++)
                {
                    GameObject obj = objectsToScatter[i];
                    if (obj != null)
                    {
                        sequence.Join(obj.transform.DOMove(originalPositions[i], returnDuration));
                    }
                }
            });
        }

        private static void OnExplosionComplete(BlockController blockController)
        {
            blockController.gameObject.SetActive(false);
        }
    }
}
