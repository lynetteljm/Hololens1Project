using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the gestures that are available
/// </summary>
// Introduction to Gestures & Voice: Exercise 5.7
/************************************************************/
// Implement the "ISpeechHandler" interface
public class ModelGestureHandler : MonoBehaviour, IManipulationHandler, ISpeechHandler
{
    /*
     * Serializable
     */
    [Tooltip("The multiplier for the rotation gesture")]
    [SerializeField]
    private float rotationSensitivity = 100.0f;
  

    /*
     * Members
     */
    // Introduction to Gestures & Voice: Exercise 4.3
    /************************************************************/
    // Set "isRotationMode" to false so that movement is the default
    private bool isRotationMode = false;           // Whether we are in rotation or movement mode
    
    private Vector3 originalPosition;                          // The starting location for the movement gesture

    // <summary>
    /// Called when the manipulation event has started
    /// </summary>
    /// <param name="eventData">Maniuplation action event info</param>
    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        // Introduction to Gestures & Voice: Exercise 3.6.a
        /***********************************************************/
        // Register the gameObject with the InputManager to receive manipulation events even when object is not in focus
       // InputManager.Instance.AddGlobalListener(gameObject);
        
        // Introduction to Gestures & Voice: Exercise 4.2.a
        /************************************************************/
        // Check that "isRotationMode" is false so that we only do movement stuff when we intend to rotate
        if (isRotationMode == false)
        {
            // Introduction to Gestures & Voice: Exercise 4.2.b
            /************************************************************/
            // Save our original position into "originalPosition"
            originalPosition = transform.position;
        }
    }
    /// <summary>
    /// Called when the manipulation event is ongoing
    /// </summary>
    /// <param name="eventData">Maniuplation action event info</param>
    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        // Introduction to Gestures & Voice: Exercise 4.2.c
        /************************************************************/
        // Check that "isRotationMode" is true so that we only do rotation stuff when we intend to rotate
        if (isRotationMode)
        {
            // Introduction to Gestures & Voice: Exercise 3.5
            /************************************************************/
            // Rotate the model based on the gesture input, “rotationSensitivity” and delta time 
            transform.Rotate(0.0f, -(eventData.CumulativeDelta.x + eventData.CumulativeDelta.z) * rotationSensitivity * Time.deltaTime, 0.0f);
        }
        else if (isRotationMode==false)    // Only do movement stuff if we intend to move
        {
            // Introduction to Gestures & Voice: Exercise 4.2.d
            /************************************************************/
            // Move the model based on navigation input offset from the "originalPosition" we saved
            transform.position = originalPosition + eventData.CumulativeDelta;
        }

        // If we're on the network
        if (NetworkPlayer.Instance != null)
        {
            // Introduction to Networked Experiences: Exercise 6.2
            /************************************************************/
            // Call NetworkPlayer's "CmdUpdateModelTransform()" and pass it the local position and rotation
            NetworkPlayer.Instance.CmdUpdateModelTransform(transform.localPosition, transform.localRotation);
        }


    }

    /// <summa>
    /// Called when the manipulation event has ended
    /// </summary>
    /// <param name="eventData">Maniuplation action event info</param>
    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        // Introduction to Gestures & Voice: Exercise 3.6.b
        /************************************************************/
        // Deregister the gameObject with the InputManager to receive manipulation events since we're done with the gesture
        InputManager.Instance.RemoveGlobalListener(gameObject);

        // If we're on the network
       // if (NetworkPlayer.Instance != null)
      //  {
            // Introduction to Networked Experiences: Exercise 6.2
            /************************************************************/
            // Call NetworkPlayer's "CmdUpdateModelTransform()" and pass it the local position and rotation
          //  NetworkPlayer.Instance.CmdUpdateModelTransform(transform.localPosition, transform.localRotation);
      //  }

    }

    /// <summary>
    /// Called when the manipulation event was cancelled prematurely
    /// </summary>
    /// <param name="eventData">Maniuplation action event info</param>
    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        OnManipulationCompleted(eventData);
    }

    // Introduction to Gestures & Voice: Exercise 5.7
    /************************************************************/
    // Implement ISpeechHandler's OnSpeechKeywordRecognized()
    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        // Introduction to Gestures & Voice: Exercise 5.7.a
        /************************************************************/
        // If the “RecognizedText“ is “rotation mode”, set “isRotationMode” to true
        if (eventData.RecognizedText == "rotation mode")
        {
            isRotationMode = true;
            
        }



        // Introduction to Gestures & Voice: Exercise 5.7.b
        /************************************************************/
        // If the “RecognizedText“ is “movement mode”, set “isRotationMode” to false
        else if (eventData.RecognizedText == "movement mode")
        {
            isRotationMode = false;
            
        }

    }




}
