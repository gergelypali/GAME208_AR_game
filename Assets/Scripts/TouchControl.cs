using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TouchControl : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public SurfaceManager surfaceManager;
    Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public Material origMaterial;

    public GameObject prefabToSpawn;
    GameObject spawnedObject;

    // Update is called once per frame
    void Update()
    {
        if (!GetTouchPosition(out touchPosition))
        {
            // if there is no touch event just return
            return;
        }

        // raycast to interact the instantiated model(the cards) on the screen
        Ray touchRay = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit touchHit;
        if (Physics.Raycast(touchRay, out touchHit))
        {
            // filter to only handle the cards
            var card = touchHit.transform.gameObject.GetComponent<CardHandler>();
            if (card != null)
            {
                card.cardTouched();
            }
        }

        if (surfaceManager.PlayingFieldPlane != null)
        {
            return;
        }

        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (spawnedObject == null)
                spawnedObject = Instantiate(prefabToSpawn, hitPose.position, hitPose.rotation);
            else
                spawnedObject.transform.position = hitPose.position;
            surfaceManager.LockPlane(hits[0].trackableId);
        }
    }

    bool GetTouchPosition(out Vector2 touchPosition)
    {
        // use touchphase.began to trigger this only once per touch
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = Vector2.zero;
        return false;
    }

}
