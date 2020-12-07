using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa : MonoBehaviour, IFocusable
{
    /*
     * Editor Exposed Variables
     */
    [Tooltip("If true, it will activate the sofa")]
    [SerializeField]
    public bool IsSofa;
    [Header("Displayed Text")]
    [Tooltip("The reference to the Text")]
    [SerializeField]
    private GameObject SofaText;

    /*
     * Members
     */
    private MeshRenderer meshRenderer;

    /// <summary>
    /// Initialization
    /// </summary>
    private void Awake()
    {

        SofaText.SetActive(false);
    }

    /// <summary>
    /// Called when gazed at
    /// </summary>
    public void OnFocusEnter()
    {

        SofaText.SetActive(true);
    }

    // Introduction to Gaze: Exercise 4.5.b
    /************************************************************/
    // Implement IFocusable's OnFocusExit()
    public void OnFocusExit()
    {

        SofaText.SetActive(false);
    }
    
}
