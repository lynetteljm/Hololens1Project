using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SharingWithUNET;
using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle automatic placement of posters using spatial mapping data
/// </summary>
[RequireComponent(typeof(SurfaceMeshesToPlanes))]
[RequireComponent(typeof(RemoveSurfaceVertices))]
public class AutoPlacePosters : MonoBehaviour, IInputClickHandler
{
    #region Singleton
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static AutoPlacePosters Instance { get; private set; }
    #endregion

    /*
     * Editor Exposed Variables
     */
    [Tooltip("The list of posters to use.")]
    [SerializeField]
    private List<ModelPoster> posters;

    /// <summary>
    /// Resource Initialization
    /// </summary>
    private void Awake()
    {
        #region Singleton Initialization
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than 1 Auto Place Posters instance detected! Deleting this new one.");
            Destroy(this);
            return;
        }
        #endregion
    }

    /// <summary>
    /// Initialization function
    /// </summary>
    void Start()
    {
        // Register global input clicked listener
        InputManager.Instance.AddGlobalListener(gameObject);

        // Introduction to Spatial Mapping: Exercise 5.17.a
        /*****************************************************/
        // Register OnPlanesCreated to handle the MakePlanesComplete event
        SurfaceMeshesToPlanes.Instance.MakePlanesComplete += OnPlanesCreated;
  
    }

    /// <summary>
    /// Deinitialization
    /// </summary>
    private void OnDestroy()
    {
        #region Singleton Deinitialization
        if (Instance == this)
        {
            Instance = null;
        }
        #endregion
    }
    
    /// <summary>
    /// Handles an air tap event
    /// </summary>
    /// <param name="eventData">The event data pertaining to the air tap</param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        // Do not doing anything if we are networked
        if (NetworkDiscoveryWithAnchors.Instance != null)
        {
            return;
        }

        // We no longer need to accept a global click event so let's deregister the global listener
        InputManager.Instance.RemoveGlobalListener(gameObject);

        // Begin auto placing
        BeginAutoPlacement();
    }
    
    /// <summary>
    /// Function that initializes the auto placement process
    /// </summary>
    public void BeginAutoPlacement()
    {
        // Introduction to Spatial Mapping: Exercise 5.11.a
        /************************************************************/
        // Check if the SpatialMappingManager is currently observing for spatial data
        if (SpatialMappingManager.Instance.IsObserverRunning())
        {
            // Introduction to Spatial Mapping: Exercise 5.11.b
            /************************************************************/
            // If so, stop it so that we can get a finished mesh for analysis
            SpatialMappingManager.Instance.StopObserver();
        }

        // Introduction to Spatial Mapping: Exercise 5.11.c
        /************************************************************/
        // Use the SurfaceMeshesToPlanes component to make planes
        SurfaceMeshesToPlanes.Instance.MakePlanes();
    }

    /// <summary>
    /// Handler for the SurfaceMeshesToPlanes.MakePlanesComplete event
    /// </summary>
    /// <param name="source">Source of the event.</param>
    /// <param name="args">Args for the event.</param>
    private void OnPlanesCreated(object source, EventArgs args)
    {
        // Introduction to Spatial Mapping: Exercise 5.17.b.i
        /*****************************************************/
        // Let's reduce our triangle count by removing triangles from SpatialMapping meshes that intersect with our active planes.
        // Call RemoveSurfaceVertices.Instance.RemoveVertices() and Pass in all activePlanes found by SurfaceMeshesToPlanes.Instance.
        RemoveSurfaceVertices.Instance.RemoveSurfaceVerticesWithinBounds(SurfaceMeshesToPlanes.Instance.ActivePlanes);

        // Introduction to Spatial Mapping: Exercise 5.17.b.ii
        /*****************************************************/
        // Since we now have the planes to represent the 3D space, let's stop rendering the ugly spatial mesh
        // Set the DrawVisualMeshes property of the SpatialMappingManager
        SpatialMappingManager.Instance.DrawVisualMeshes = false;

        // Collection of wall planes that we can use to set vertical items on.
        List<GameObject> wallPlanes = new List<GameObject>();

        // Introduction to Spatial Mapping: Exercise 6.1.a
        /*****************************************************/
        // Get all wall planes by calling SurfaceMeshesToPlanes.Instance.GetActivePlanes().
        // Assign the result to the 'wallPlanes' list we created in the line above
        wallPlanes = SurfaceMeshesToPlanes.Instance.GetActivePlanes(PlaneTypes.Wall);


        // Introduction to Spatial Mapping: Exercise 6.1.b
        /*****************************************************/
        // We are all done processing the mesh, so we can now position posters in the world
        // and use surface planes to set their starting positions.
        // To do this, call GenerateItemsInWorld() and pass in the lists of posters and wall planes that we found earlier.
        GenerateItemsInWorld(posters, wallPlanes);
    }

    /// <summary>
    /// Creates and positions a collection of gameobjects on SurfacePlanes in the environment.
    /// </summary>
    /// <param name="posterList">Collection of prefab GameObjects that have the Placeable component.</param>
    /// <param name="surfaces">Collection of SurfacePlane objects in the world.</param>
    private void GenerateItemsInWorld(List<ModelPoster> posterList, List<GameObject> surfaces)
    {
        List<int> UsedPlanes = new List<int>();

        // This is a simple sorting function that sorts all the surfaces in the list according to their distance from our user. Don't worry too much about it.
        surfaces.Sort((lhs, rhs) =>
        {
            Vector3 headPosition = Camera.main.transform.position;
            Collider rightCollider = rhs.GetComponent<Collider>();
            Collider leftCollider = lhs.GetComponent<Collider>();

            // This plane is big enough, now we will evaluate how far the plane is from the user's head.  
            // Since planes can be quite large, we should find the closest point on the plane's bounds to the 
            // user's head, rather than just taking the plane's center position.
            Vector3 rightSpot = rightCollider.ClosestPointOnBounds(headPosition);
            Vector3 leftSpot = leftCollider.ClosestPointOnBounds(headPosition);

            return Vector3.Distance(leftSpot, headPosition).CompareTo(Vector3.Distance(rightSpot, headPosition));
        });

        // Now for every poster we want to stick in the world...
        foreach (ModelPoster poster in posterList)
        {
            /*
             * Placing the Poster
             */
            // Try to find a plane which we can place this poster by comparing their collider sizes
            int index = FindNearestPlane(surfaces, poster.GetComponent<Collider>().bounds.size, UsedPlanes);

            // If we do find a good plane we can do something smarter.
            if (index >= 0)
            {
                // Mark that this plane has already been used so that we won't use it again
                UsedPlanes.Add(index);

                // Get a reference to that plane that we've found and use it's info to position the poster on the plane
                GameObject surface = surfaces[index];
                SurfacePlane plane = surface.GetComponent<SurfacePlane>();

                // Introduction to Spatial Mapping: Exercise 6.3.a
                /*****************************************************/
                // Position the poster and take the plane’s thickness & normal into consideration
                poster.transform.position = surface.transform.position + (plane.SurfaceNormal * plane.PlaneThickness * 0.5f);

                // Introduction to Spatial Mapping: Exercise 6.3.b
                /*****************************************************/
                // Orientate the poster and take the plane's surface normal into consideration
                poster.transform.forward = plane.SurfaceNormal;
            }

            // If we don't, then leave the posters in their original position
        }

        // Check if we are networked, if so we need to do a sync
        if (NetworkPlayer.Instance != null)
        {
            PosterManager.Instance.SyncPosters();
        }
    }

    /// <summary>
    /// Attempts to find a the closest plane to the user which is large enough to fit the object.
    /// </summary>
    /// <param name="planes">List of planes to consider for object placement.</param>
    /// <param name="posterSize">Minimum size that the plane is required to be.</param>
    /// <param name="startIndex">Index in the planes collection that we want to start at (to help avoid double-placement of objects).</param>
    /// <returns>Index of the nearest plane in planes that fits the minSize provided</returns>
    private int FindNearestPlane(List<GameObject> planes, Vector3 posterSize, List<int> usedPlanes)
    {
        // Look through all the list of planes
        for (int i = 0; i < planes.Count; i++)
        {
            // Check if we have used this plane before
            if (usedPlanes.Contains(i))
            {
                continue;
            }

            // Obtain the plane's collider and use it to calculate to see if it is big enough for our poster
            Collider collider = planes[i].GetComponent<Collider>();

            // Introduction to Spatial Mapping: Exercise 6.2
            /*****************************************************/
            // Check the collider's bounds's size values with the poster size to see if it is too small
            if (posterSize.x > collider.bounds.size.x||posterSize.y>collider.bounds.size.y)
            {
                // This plane is too small to fit our vertical object.
                continue;
            }

            // It is big enough so let's use this!
            return i;
        }

        // Return -1 if there is no planes available to fit on
        return -1;
    }
}
