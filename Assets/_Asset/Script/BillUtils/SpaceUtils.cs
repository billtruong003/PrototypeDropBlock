using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BillUtils.SpaceUtils
{
    public static class SpaceUtilities
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

        public static Vector3 GetMeshWorldPosition(GameObject obj)
        {
            MeshRenderer meshRenderer = obj.GetComponentInChildren<MeshRenderer>();
            MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();

            if (meshRenderer != null && meshFilter != null)
            {
                return meshRenderer.bounds.center;
            }
            return Vector3.zero;
        }

        // Method to get the world bounds of the mesh
        public static Bounds GetMeshWorldBounds(GameObject obj)
        {
            MeshFilter meshFilter = obj.GetComponentInChildren<MeshFilter>();
            if (meshFilter == null)
            {
                return new Bounds(Vector3.zero, Vector3.zero);
            }

            Mesh mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                return new Bounds(Vector3.zero, Vector3.zero);
            }

            // Calculate the mesh bounds in local space
            Bounds bounds = mesh.bounds;

            // Transform the local bounds to world bounds
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = meshFilter.transform.TransformPoint(vertices[i]);
            }

            Bounds worldBounds = new Bounds(vertices[0], Vector3.zero);
            for (int i = 1; i < vertices.Length; i++)
            {
                worldBounds.Encapsulate(vertices[i]);
            }

            return worldBounds;
        }

        public static Bounds CalculateBounds(List<GameObject> gameObjects)
        {
            Bounds bounds = new Bounds(gameObjects[0].transform.position, Vector3.zero);

            foreach (GameObject go in gameObjects)
            {
                if (go != null)
                {
                    bounds.Encapsulate(go.GetComponent<Renderer>().bounds);
                }
            }

            return bounds;
        }
        private static Vector3 GetMeshContainerCenter(Transform container)
        {
            MeshFilter[] meshFilters = container.GetComponentsInChildren<MeshFilter>();

            if (meshFilters.Length == 0)
            {
                Debug.LogWarning("No meshes found in the container!");
                return container.position;
            }

            Bounds bounds = meshFilters[0].mesh.bounds;
            bounds.center = meshFilters[0].transform.position;

            foreach (MeshFilter mf in meshFilters)
            {
                Bounds worldBounds = mf.mesh.bounds;
                worldBounds.center = mf.transform.position;
                bounds.Encapsulate(worldBounds);
            }

            return bounds.center;
        }
    }
    public static class VectorUtils
    {
        public static Vector3 HalfY = new Vector3(0, 0.5f, 0);
    }
}
