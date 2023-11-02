using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class Instantiate : MonoBehaviour
{
    [Tooltip(
        "Transform whose position will be used to place the instantiated prefab. If not provided, will use camera forward.")]
    public Transform positionTransform;

    [Tooltip("The prefab to use to spawn objects.")]
    public GameObject Prefab;
    // public GameObject Capsule;
    // public GameObject Cube;

    public ARRaycastManager raycastManager;

    private bool showSpawnedObjs;
    private bool showTackedImages;
    List<GameObject> instantiatedObjects = new List<GameObject>();
    [SerializeField] public Image imageUserGuide;

    void Start()
    {
        showSpawnedObjs = false;
        showTackedImages = false;
        if (raycastManager == null)
        {
            raycastManager = GetComponent<ARRaycastManager>();
        }

        imageUserGuide.gameObject.SetActive(false);
    }

    public void InstantiateObj()
    {
        Vector3 spawnPosition;
        Quaternion spawnRotation;
        
        spawnPosition = positionTransform.position;
        spawnRotation = positionTransform.rotation;
        GameObject go = Instantiate(Prefab, spawnPosition, spawnRotation, transform);

        if (go.GetComponent<ARAnchor>() == null)
        {
            go.AddComponent<ARAnchor>();
        }

        instantiatedObjects.Add(go);
    }

    public void InstantiateCameraObj()
    {
        Vector3 spawnPosition;
        Quaternion spawnRotation;

        positionTransform = Camera.main.transform;
        spawnPosition = positionTransform.position + positionTransform.forward.normalized * 1f;
        spawnRotation = Quaternion.LookRotation(positionTransform.forward, positionTransform.up);

        GameObject go = Instantiate(Prefab, spawnPosition, spawnRotation, transform);

        if (go.GetComponent<ARAnchor>() == null)
        {
            go.AddComponent<ARAnchor>();
        }

        instantiatedObjects.Add(go);
    }

    public void ToggleVisibilitySpawnedObjects()
    {
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("SpawnedImage");
        foreach (GameObject gameObject in foundObjects)
        {
            var objlist = gameObject.GetComponentsInChildren<Renderer>();
            foreach (var obj in objlist)
            {
                obj.gameObject.GetComponentInChildren<Renderer>().enabled = showSpawnedObjs;
            }
        }
        
        // foreach (GameObject go in (instantiatedObjects))
        // {
        //     // go.SetActive(showSpawnedObjs);
        //     go.GetComponentInChildren<Renderer>().enabled = showSpawnedObjs;
        // }

        // foreach (Transform child in transform)
        // {
        //     //child.gameObject.SetActive(!child.gameObject.activeSelf);
        //     var objlist = child.gameObject.GetComponentsInChildren<Renderer>();
        //     
        //     foreach (var obj in objlist)
        //     {
        //         obj.gameObject.GetComponentInChildren<Renderer>().enabled = showSpawnedObjs;
        //     }
        // }
        showSpawnedObjs = !showSpawnedObjs;
    }

    public void ToggleTrackedImages()
    {
        GameObject foundObject = GameObject.FindWithTag("TackedImage");
        var objlist = foundObject.gameObject.GetComponentsInChildren<Renderer>();

        foreach (var obj in objlist)
        {
            obj.gameObject.GetComponentInChildren<Renderer>().enabled = showTackedImages;
        }

        showTackedImages = !showTackedImages;
    }

    public void ToggleUserGuide()
    {
        bool isActive = imageUserGuide.gameObject.activeSelf;
        // Toggle the visibility
        imageUserGuide.gameObject.SetActive(!isActive);
    }
}