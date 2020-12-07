using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Introduction to Gaze: Exercise 4.5.b
/************************************************************/
// Implement IFocusable
// Introduction to Gestures & Voice: Exercise 2.5.a
/************************************************************/
// Implement IInputClickHandler
/// <summary>
/// Component that controls a poster that activates a model
/// </summary>
public class ModelPoster : MonoBehaviour,IFocusable, IInputClickHandler
{
    /*
     * Editor Exposed Variables
     */
    [Tooltip("If true, it will activate the HoloLens")]
    [SerializeField]
    public bool IsHoloLens;
    [Tooltip("If true, it will activate the car")]
    [SerializeField]
    public bool IsCar;
    [Tooltip("If true, it will activate the engine")]
    [SerializeField]
    public bool IsEngine;

    /*
     * Members
     */
    private MeshRenderer meshRenderer;

    /// <summary>
    /// Initialization
    /// </summary>
    private void Awake()
    {
        // Get Components
        meshRenderer = GetComponent<MeshRenderer>();

        // Introduction to Gaze: Exercise 4.5.a
        /************************************************************/
        // Set default colour to gray
        meshRenderer.material.color = Color.gray;
        
    }

    // Introduction to Gaze: Exercise 4.5.b
    /************************************************************/
    // Implement IFocusable's OnFocusEnter()
    /// <summary>
    /// Called when gazed at
    /// </summary>
    public void OnFocusEnter()
    {

        // Introduction to Gaze: Exercise 4.5.b.i
        /************************************************************/
        // OnFocusEnter(): 
        // Change colour of the object to white when it is gazed at
        meshRenderer.material.color = Color.white;
    }


    // Introduction to Gaze: Exercise 4.5.b
    /************************************************************/
    // Implement IFocusable's OnFocusExit()
    public void OnFocusExit()
    {
        // Introduction to Gaze: Exercise 4.5.b.ii
        /************************************************************/
        // OnFocusExit(): 
        // Change colour of the object to gray when it is stopped being gazed at
        meshRenderer.material.color = Color.gray;
    }


    // Introduction to Gestures & Voice: Exercise 2.5.a
    /************************************************************/
    // Implement IInputClickHandler's OnInputClicked()
    // Call ModelLibrary.Instance.ExecuteActionToShowModel(IsHoloLens, IsCar, IsEngine)
    public void OnInputClicked(InputClickedEventData eventData)
    {

        // Introduction to Gestures & Voice: Exercise 2.5.b
        /************************************************************/
        // OnInputClicked():
        // Call ModelLibrary.Instance.ExecuteActionToShowModel(IsHoloLens, IsCar, IsEngine) to show the correct model
        ModelLibrary.Instance.ExecuteActionToShowModel(IsHoloLens, IsCar, IsEngine);
    }    
}
