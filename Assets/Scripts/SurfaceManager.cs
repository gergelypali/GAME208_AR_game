using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SurfaceManager : MonoBehaviour
{
    public ARPlaneManager PlaneManager;
    public ARPlane PlayingFieldPlane = null;
    public Material grassMaterial;
    public Material origMaterial;

    // Update is called once per frame
    void Update()
    {
        if (PlayingFieldPlane?.subsumedBy != null)
        {
            PlayingFieldPlane = PlayingFieldPlane.subsumedBy;
        }
    }
    public void LockPlane(TrackableId keepPlane)
    {
        // Disable all planes and "deactivate" the planemanager so the game is not searching for more planes
        var arPlane = PlaneManager.GetPlane(keepPlane);
        foreach (var plane in PlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        PlayingFieldPlane = arPlane;
        PlaneManager.detectionMode = PlaneDetectionMode.None;
    }
}
