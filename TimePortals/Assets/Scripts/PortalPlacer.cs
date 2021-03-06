﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class PortalPlacer : MonoBehaviour
{
    ARRaycastManager m_ARRaycastManager;
    ARPlaneManager m_ARPlaneManager;
    List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    // List of portal GameObjects which can be instantiated
    public GameObject portal;
    public GameObject[] portalList;
    public GameObject cameraText;
    // GameObject to be spawned
    private GameObject spawnedPortal;

    private void Awake()
    {
        m_ARRaycastManager = GetComponent<ARRaycastManager>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            // Get the touch input
            Touch touch = Input.GetTouch(0);

            if (m_ARRaycastManager.Raycast(touch.position, raycastHits, TrackableType.PlaneWithinPolygon))
            {
                // Get the pose of the hit
                Pose pose = raycastHits[0].pose;

                var portalIndex = Random.Range(0, (portalList.Length + 1));

                if (spawnedPortal == null)
                {
                    spawnedPortal = Instantiate(portalList[portalIndex], pose.position, Quaternion.Euler(0, 0, 0));

                    // TODO: move following lines to private function SetPortalRotation()

                    var rotationOfPortal = spawnedPortal.transform.rotation.eulerAngles;

                    spawnedPortal.transform.eulerAngles = new Vector3(rotationOfPortal.x, Camera.main.transform.eulerAngles.y, rotationOfPortal.z);

                    // turn off the plane detector when the portal is placed
                    foreach (var plane in m_ARPlaneManager.trackables)
                    {
                        plane.gameObject.SetActive(false);
                    }

                    // hide camera text
                    cameraText.SetActive(false);
                }
                else
                {
                    spawnedPortal.transform.position = pose.position;

                    var rotationOfPortal = spawnedPortal.transform.rotation.eulerAngles;

                    spawnedPortal.transform.eulerAngles = new Vector3(rotationOfPortal.x, Camera.main.transform.eulerAngles.y, rotationOfPortal.z);
                }
            }
        }
    }

    /*
    private void SetPortalRotation(gameObject portal)
    {

    }*/
}
