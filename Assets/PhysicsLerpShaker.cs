using UnityEngine;
using DG.Tweening;

public class PhysicsLerpShaker : MonoBehaviour
{
    [SerializeField] private Transform _objectToMove;
    [SerializeField, Range(0, 1)] private float _duration = 1f; // Total duration to move along the curve
    [SerializeField] private float _launchForce = 5f; // The force of the launch
    [SerializeField] private Vector3 _launchAngle = new Vector3(45f, 0f, 0f); // The angle of the launch
    [SerializeField] private int _count = 20; // Number of points to define the curve
    [SerializeField] private bool CheatDestroy;
    [SerializeField] private bool CheatRandom;

    private void Update()
    {
        // For testing, call the LaunchObject() method on a key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheatRandom)
            {
                _launchForce = Random.Range(15, 25);
                _launchAngle = Random.insideUnitSphere * 75;
            }
            LaunchObject(_objectToMove, _launchForce, _launchAngle, _duration);
        }
    }

    private void LaunchObject(Transform objectToMove, float force, Vector3 angle, float duration)
    {
        Vector3 startPosition = CheatDestroy ? Vector3.zero : objectToMove.position;
        Vector3[] path = EvaluateParabolicPath(startPosition, force, angle, _count);
        objectToMove.position = startPosition;
        objectToMove.gameObject.SetActive(true);
        VFXManager.Instance.TriggerExplo(startPosition);

        // Create a sequence to combine path movement and rotation
        Sequence launchSequence = DOTween.Sequence();

        // Add path movement to the sequence
        launchSequence.Append(objectToMove.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.OutQuad));

        // Add shake rotation to the sequence
        launchSequence.Join(objectToMove.DOShakeRotation(duration, strength: new Vector3(180, 0, 180), vibrato: 200, randomness: 90)
            .SetEase(Ease.Linear));

        // Callback on complete
        launchSequence.OnComplete(() => OnCompletePath(objectToMove.position));
    }

    private void OnCompletePath(Vector3 endPose)
    {
        VFXManager.Instance.TriggerExplo(endPose);
        _objectToMove.gameObject.SetActive(false);
        Debug.Log("Launch Complete");
    }

    private Vector3[] EvaluateParabolicPath(Vector3 start, float force, Vector3 angle, int count)
    {
        Vector3[] points = new Vector3[count];
        Vector3 direction = Quaternion.Euler(angle) * Vector3.forward;
        float initialVelocityX = force * direction.x;
        float initialVelocityY = force * direction.y;
        float initialVelocityZ = force * direction.z;
        float totalTime = (2 * initialVelocityY) / Mathf.Abs(Physics.gravity.y); // Time of flight
        float timeStep = totalTime / (count - 1);

        for (int i = 0; i < count; i++)
        {
            float t = i * timeStep;
            float x = initialVelocityX * t;
            float y = initialVelocityY * t - 0.5f * Mathf.Abs(Physics.gravity.y) * t * t;
            float z = initialVelocityZ * t;
            points[i] = start + new Vector3(x, y, z);
        }

        return points;
    }
}
