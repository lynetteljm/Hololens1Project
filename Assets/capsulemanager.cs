using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capsulemanager : MonoBehaviour {
    #region Singleton
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static capsulemanager Instance { get; private set; }
    #endregion
    /*
    * Editor Exposed Variables
    */
    [Tooltip("Reference to capsule")]
    [SerializeField]
    private capsule cap;

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
            Debug.LogError("More than 1 Capsule Manager instance detected! Deleting this new one.");
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
    /// Function to set the transform values of the capsule
    /// </summary>
    /// <param name="Cap">If true, the capsule will be what we are positioning</param>
    /// <param name="localPosition">The local position to set the poster to</param>
    /// <param name="localRotation">The local rotation to set the poster to</param>
    /// <param name="localScale">The local scale to set the poster to</param>
    public void SetCapsuleTransform(bool Cap, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
    {
        // Decide what poster to set
        capsule capsuleToSet;
        if (Cap)
        {
            capsuleToSet = cap;
        }

        else
        {
            return;
        }

        // Set the values
        capsuleToSet.transform.localPosition = localPosition;
        capsuleToSet.transform.localRotation = localRotation;
        capsuleToSet.transform.localScale = localScale;
    }

    /// <summary>
    /// Function to do a sync of the capsule across the network
    /// </summary>
    public void SyncCapsule()
    {
        NetworkPlayer.Instance.CmdSetCapsuleTransform(true, cap.transform.localPosition, cap.transform.localRotation, cap.transform.localScale);
    }

    /// <summary>
    /// Function to do a sync of the capsule across the network
    /// </summary>
    public void RequestSyncCapsule()
    {
        NetworkPlayer.Instance.RpcSetCapsuleTransform(true, cap.transform.localPosition, cap.transform.localRotation, cap.transform.localScale);
    }

}
