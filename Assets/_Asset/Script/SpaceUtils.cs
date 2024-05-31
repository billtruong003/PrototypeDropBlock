using UnityEngine;
using System.Collections.Generic;

namespace BillUtils.SpaceUtilities
{
    public static class SpaceUtils
    {
        /// <summary>
        /// Returns the midpoint between two points.
        /// </summary>
        public static Vector3 MidpointBetweenPoints(Vector3 pointA, Vector3 pointB)
        {
            return (pointA + pointB) / 2;
        }

        /// <summary>
        /// Checks if two vectors are approximately equal within a given tolerance.
        /// </summary>
        public static bool ApproximatelyEqual(Vector3 vectorA, Vector3 vectorB, float tolerance = 0.0001f)
        {
            return (vectorA - vectorB).sqrMagnitude < tolerance * tolerance;
        }

        /// <summary>
        /// Calculates the centroid of a set of points.
        /// </summary>
        public static Vector3 CalculateCentroid(Vector3[] points)
        {
            if (points == null || points.Length == 0)
                return Vector3.zero;

            Vector3 centroid = Vector3.zero;
            foreach (Vector3 point in points)
            {
                centroid += point;
            }
            return centroid / points.Length;
        }

        /// <summary>
        /// Returns a random point inside a sphere with the given radius.
        /// </summary>
        public static Vector3 RandomPointInsideSphere(float radius)
        {
            return Random.insideUnitSphere * radius;
        }

        /// <summary>
        /// Returns a random point on the surface of a unit sphere.
        /// </summary>
        public static Vector3 RandomPointOnUnitSphere()
        {
            return Random.onUnitSphere;
        }

        /// <summary>
        /// Returns a random point inside a unit circle.
        /// </summary>
        public static Vector3 RandomPointInsideUnitCircle()
        {
            return Random.insideUnitCircle;
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        public static float DistanceBetweenPoints(Vector3 pointA, Vector3 pointB)
        {
            return (pointA - pointB).magnitude;
        }

        /// <summary>
        /// Determines which point in the list is closest to the reference point.
        /// </summary>
        public static Vector3 ClosestPoint(Vector3 referencePoint, List<Vector3> points)
        {
            if (points == null || points.Count == 0)
                return Vector3.zero;

            Vector3 closestPoint = points[0];
            float closestDistance = DistanceBetweenPoints(referencePoint, closestPoint);

            for (int i = 1; i < points.Count; i++)
            {
                float distance = DistanceBetweenPoints(referencePoint, points[i]);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = points[i];
                }
            }

            return closestPoint;
        }
    }
}
