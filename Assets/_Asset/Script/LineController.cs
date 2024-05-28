using UnityEngine;
using DG.Tweening;
using AnimationController.WithTransform;
public class LineController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private BlockController blockController;

    [SerializeField] private float trailLength = 10f;
    [SerializeField] private float startWidth = 0.1f;
    [SerializeField] private float endWidth = 0.1f;

    [SerializeField] private Color startColor = Color.red;
    [SerializeField] private Color endColor = Color.clear;
    [SerializeField] private float textureScrollSpeed = 1.0f;
    private void Start()
    {
        // AddLineRenderer();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (!blockController.DoneDrop)
        {
            ShootLineDownwards();
            ScrollTexture();
        }
        else
        {
            DisableLine();
        }
    }


    private void ShootLineDownwards()
    {
        lineRenderer.enabled = true; // Enable the line

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x, transform.position.y - trailLength, transform.position.z);

        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }

    private void ScrollTexture()
    {
        float offset = Time.time * textureScrollSpeed;
        lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }

    private void DisableLine()
    {
        lineRenderer.enabled = false; // Disable the line
    }

}
