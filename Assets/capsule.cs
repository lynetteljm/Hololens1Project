using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capsule : MonoBehaviour, IFocusable, IInputClickHandler
{
    /*
     * Editor Exposed Variables
     */
    [Tooltip("If true, it will activate the capsule")]
    [SerializeField]
    public bool IsCapsule;
    [Header("Displayed Text")]
    [Tooltip("The reference to the Text")]
    [SerializeField]
    private GameObject CapsuleText;
    [Tooltip("The reference to the Panel")]
    [SerializeField]
    private GameObject CapsulePanel;

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
        CapsuleText.SetActive(false);
        CapsulePanel.SetActive(false);
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
        meshRenderer.material.color = Color.red;
        CapsuleText.SetActive(true);
        CapsulePanel.SetActive(true);
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
        CapsuleText.SetActive(false);
        CapsulePanel.SetActive(false);
    }
    public void OnInputClicked(InputClickedEventData eventData)
    {
        meshRenderer.material.color = Color.yellow;
        
    }
}