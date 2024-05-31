using System.Collections.Generic;
using UnityEngine;

public class VisualGuide : Singleton<VisualGuide>
{
    [SerializeField] private List<VisualGuideController> visualGuideControllers = new List<VisualGuideController>();
    [SerializeField, Range(0.05f, 0.1f)] private float yDistanceUp;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("VisualGuide Awake: Instance set");
    }

    private void Start()
    {
        Debug.Log("VisualGuide Start: Số lượng visual guides ban đầu: " + visualGuideControllers.Count);
    }

    public VisualGuideController GetAvailableVisualGuide()
    {
        Debug.Log("Getting available visual guide. Current count: " + visualGuideControllers.Count);
        if (visualGuideControllers.Count > 0)
        {
            VisualGuideController vsGuidePrefab = visualGuideControllers[0];
            visualGuideControllers.RemoveAt(0);
            vsGuidePrefab.gameObject.SetActive(true);
            Debug.Log("Visual guide retrieved and activated. Remaining count: " + visualGuideControllers.Count);
            return vsGuidePrefab;
        }
        Debug.LogWarning("No available visual guides");
        return null;
    }

    public void AddBackVisualGuide(VisualGuideController vsController)
    {
        visualGuideControllers.Add(vsController);
        vsController.gameObject.SetActive(false);
        Debug.Log("Visual guide added back and deactivated. Current count: " + visualGuideControllers.Count);
    }
}
