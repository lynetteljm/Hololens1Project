using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaManager : MonoBehaviour
{
    #region Singleton
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static SofaManager Instance { get; private set; }
    #endregion

    /*
    * Editor Exposed Variables
    */
    [Tooltip("Reference to sofa")]
    [SerializeField]
    private Sofa yellowsofa;

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
            Debug.LogError("More than 1 Sofa Manager instance detected! Deleting this new one.");
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
    /// <param name="YellowSofa">If true, the sofa will be what we are positioning</param>
    /// <param name="localPosition">The local position to set the poster to</param>
    /// <param name="localRotation">The local rotation to set the poster to</param>
    /// <param name="localScale">The local scale to set the poster to</param>
    public void SetSofaTransform(bool YellowSofa, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
    {
        // Decide what poster to set
        Sofa sofaToSet;
        if (YellowSofa)
        {
            sofaToSet = yellowsofa;
        }

        else
        {
            return;
        }

        // Set the values
        sofaToSet.transform.localPosition = localPosition;
        sofaToSet.transform.localRotation = localRotation;
        sofaToSet.transform.localScale = localScale;

    }

    /// <summary>
    /// Function to do a sync of the sofa across the network
    /// </summary>
   public void SyncSofa()
    {
        NetworkPlayer.Instance.CmdSetSofaTransform(true, yellowsofa.transform.localPosition, yellowsofa.transform.localRotation, yellowsofa.transform.localScale);
    }
    /// <summary>
    /// Function to do a sync of the capsule across the network
    /// </summary>
    public void RequestSyncSofa()
   {
        NetworkPlayer.Instance.RpcSetSofaTransform(true, yellowsofa.transform.localPosition, yellowsofa.transform.localRotation, yellowsofa.transform.localScale);
    }
}