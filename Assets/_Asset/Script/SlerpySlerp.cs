using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlerpySlerp : MonoBehaviour
{
    [SerializeField] private Transform _start, _center, _end;
    [SerializeField] private int _count = 15;
    [SerializeField] private Transform _objectToMove;
    [SerializeField] private float _duration = 0.5f; // Total duration to move along the curve
    [SerializeField] private bool useBezierCurve = false; // Toggle between Slerp and Bezier curve

    private void Update()
    {
        // For testing, call the ShootObject() method on a key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ShootObject(_objectToMove, _duration));
        }
    }

    // private void OnDrawGizmos()
    // {
    //     var points = useBezierCurve
    //         ? EvaluateBezierPoints(_start.position, _center.position, _end.position, _count)
    //         : EvaluateSlerpPoints(_start.position, _end.position, _center.position, _count);

    //     foreach (var point in points)
    //     {
    //         Gizmos.DrawSphere(point, 0.1f);
    //     }

    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(_center.position, 0.5f);
    //     Gizmos.DrawSphere(_start.position, 0.5f);
    //     Gizmos.DrawSphere(_end.position, 0.5f);
    // }

    private List<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end, Vector3 center, int count)
    {
        var points = new List<Vector3>();
        var startRelativeCenter = start - center;
        var endRelativeCenter = end - center;
        var f = 1f / count;

        for (var i = 0f; i <= 1f; i += f)
        {
            points.Add(Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + center);
        }

        return points;
    }

    private List<Vector3> EvaluateBezierPoints(Vector3 start, Vector3 control, Vector3 end, int count)
    {
        var points = new List<Vector3>();
        var f = 1f / count;

        for (var i = 0f; i <= 1f; i += f)
        {
            points.Add(BezierLerp(start, control, end, i));
        }

        return points;
    }

    private Vector3 BezierLerp(Vector3 start, Vector3 control, Vector3 end, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * start; // u^2 * P0
        p += 2 * u * t * control; // 2 * u * t * P1
        p += tt * end; // t^2 * P2

        return p;
    }

    private IEnumerator ShootObject(Transform objectToMove, float duration)
    {
        var points = useBezierCurve
            ? EvaluateBezierPoints(_start.position, _center.position, _end.position, _count)
            : EvaluateSlerpPoints(_start.position, _end.position, _center.position, _count);

        if (points.Count < 2)
        {
            yield break;
        }

        _objectToMove.position = points[0];
        _objectToMove.gameObject.SetActive(true);
        VFXManager.Instance.TriggerExplo(_objectToMove.position);

        float segmentDuration = duration / (points.Count - 1);

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 start = points[i];
            Vector3 end = points[i + 1];

            for (float t = 0; t < 1f; t += Time.deltaTime / segmentDuration)
            {
                objectToMove.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
            objectToMove.position = end;
        }

        VFXManager.Instance.TriggerExplo(_objectToMove.position);
        _objectToMove.gameObject.SetActive(false);
    }
}
