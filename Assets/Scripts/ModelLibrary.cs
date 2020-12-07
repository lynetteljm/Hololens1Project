using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that helps to hide all the models
/// </summary>
public class ModelLibrary : MonoBehaviour
{
    #region Singleton
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static ModelLibrary Instance { get; private set; }
    #endregion

    /*
     * Editor Exposed Variables
     */
    [Header("Configuration")]
    [Tooltip("The reference to the HoloLens Game Object")]
    [SerializeField]
    private GameObject hololensModel;
    [Header("Configuration")]
    [Tooltip("The reference to the HoloLens Game Object")]
    [SerializeField]
    private GameObject carModel;
    [Header("Configuration")]
    [Tooltip("The reference to the HoloLens Game Object")]
    [SerializeField]
    private GameObject engineModel;  

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
            Debug.LogError("More than 1 Model Library instance detected! Deleting this new one.");
            Destroy(this);
            return;
        }
        #endregion

        // Error Checks
        if (hololensModel == null)
        {
            Debug.LogError("ModelLibrary: HoloLens model not set.");
        }
        if (carModel == null)
        {
            Debug.LogError("ModelLibrary: Car model not set.");
        }
        if (engineModel == null)
        {
            Debug.LogError("ModelLibrary: Engine model not set.");
        }

        // Show the hololens at the start
        ExecuteActionToShowModel(true, false, false);
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
    /// Function to hide all models
    /// </summary>
    public void HideAllModels()
    {
        hololensModel.SetActive(false);
        carModel.SetActive(false);
        engineModel.SetActive(false);
    }

    /// <summary>
    /// Function to get the current active state of the HoloLens model
    /// </summary>
    /// <returns>Whether or not the HoloLens model is active</returns>
    public bool IsHoloLensActive()
    {
        return hololensModel.activeSelf;
    }

    /// <summary>
    /// Function to get the current active state of the Car model
    /// </summary>
    /// <returns>Whether or not the Car model is active</returns>
    public bool IsCarActive()
    {
        return carModel.activeSelf;
    }

    /// <summary>
    /// Function to get the current active state of the Engine model
    /// </summary>
    /// <returns>Whether or not the Engine model is active</returns>
    public bool IsEngineActive()
    {
        return engineModel.activeSelf;
    }

    /// <summary>
    /// Function to request changing of showing the model based on the parameters provided
    /// </summary>
    /// <param name="hololens">Whether to show the hololens model</param>
    /// <param name="car">Whether to show the car model</param>
    /// <param name="engine">Whether to show the engine model</param>
    public void ExecuteActionToShowModel(bool hololens, bool car, bool engine)
    {
        // Introduction to Networked Experiences: Exercise 5.2.a
        /************************************************************/
        // Check if we are not networked by checking for the lack of the "NetworkPlayer" instance
        if (NetworkPlayer.Instance==null)
        {
            // Introduction to Networked Experiences: Exercise 5.2.b
            /************************************************************/
            // If we are indeed not networked, continue to call ShowModel()
            ShowModel(hololens, car, engine);
        }
        else
        {
            // Introduction to Networked Experiences: Exercise 5.2.c
            /************************************************************/
            // If not, call NetworkPlayer's "CmdShowModel"
            NetworkPlayer.Instance.CmdShowModel(hololens, car, engine);
        }
    }

    /// <summary>
    /// Function to show the model based on the parameters provided 
    /// </summary>
    /// <param name="hololens">Whether to show the hololens model</param>
    /// <param name="car">Whether to show the car model</param>
    /// <param name="engine">Whether to show the engine model</param>
    public void ShowModel(bool hololens, bool car, bool engine)
    {
        HideAllModels();

        if (hololens)
        {
            hololensModel.SetActive(true);
        }
        else if (car)
        {
            carModel.SetActive(true);
        }
        else if (engine)
        {
            engineModel.SetActive(true);
        }
    }

}
