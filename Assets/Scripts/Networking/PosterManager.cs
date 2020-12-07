using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterManager : MonoBehaviour
{
    #region Singleton
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static PosterManager Instance { get; private set; }
    #endregion

    /*
     * Editor Exposed Variables
     */
    [Tooltip("Reference to the HoloLens poster")]
    [SerializeField]
    private ModelPoster holoLensPoster;
    [Tooltip("Reference to the HoloLens poster")]
    [SerializeField]
    private ModelPoster carPoster;
    [Tooltip("Reference to the HoloLens poster")]
    [SerializeField]
    private ModelPoster enginePoster;

    /// <summary>
    /// Initialization
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
            Debug.LogError("More than 1 Poster Manager instance detected! Deleting this new one.");
            Destroy(this);
            return;
        }
        #endregion
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
    /// Function to set the transform values of the posters
    /// </summary>
    /// <param name="holoLens">If true, the hololens will be the poster we are positioning</param>
    /// <param name="car">If true, the car will be the poster we are positioning</param>
    /// <param name="engine">If true, the engine will be the poster we are positioning</param>
    /// <param name="localPosition">The local position to set the poster to</param>
    /// <param name="localRotation">The local rotation to set the poster to</param>
    public void SetPosterTransform(bool holoLens, bool car, bool engine, Vector3 localPosition, Quaternion localRotation)
    {
        // Decide what poster to set
        ModelPoster posterToSet;
        if (holoLens)
        {
            posterToSet = holoLensPoster;
        }
        else if (car)
        {
            posterToSet = carPoster;
        }
        else if (engine)
        {
            posterToSet = enginePoster;
        }
        else
        {
            return;
        }

        // Set the values
        posterToSet.transform.localPosition = localPosition;
        posterToSet.transform.localRotation = localRotation;
    }

    /// <summary>
    /// Function to do a sync of the posters across the network
    /// </summary>
    public void SyncPosters()
    {
        // If this is called, we should be networked already
        NetworkPlayer.Instance.CmdSetPosterTransform(true, false, false, holoLensPoster.transform.localPosition, holoLensPoster.transform.localRotation);
        NetworkPlayer.Instance.CmdSetPosterTransform(false, true, false, carPoster.transform.localPosition, carPoster.transform.localRotation);
        NetworkPlayer.Instance.CmdSetPosterTransform(false, false, true, enginePoster.transform.localPosition, enginePoster.transform.localRotation);
    }

    /// <summary>
    /// Function to do a sync of the posters across the network
    /// </summary>
    public void RequestSyncPosters()
    {
        // If this is called, we should be networked already
        NetworkPlayer.Instance.RpcSetPosterTransform(true, false, false, holoLensPoster.transform.localPosition, holoLensPoster.transform.localRotation);
        NetworkPlayer.Instance.RpcSetPosterTransform(false, true, false, carPoster.transform.localPosition, carPoster.transform.localRotation);
        NetworkPlayer.Instance.RpcSetPosterTransform(false, false, true, enginePoster.transform.localPosition, enginePoster.transform.localRotation);
    }
}
