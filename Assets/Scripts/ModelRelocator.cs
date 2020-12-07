using HoloToolkit.Unity.InputModule;
using UnityEngine;
/// <summary>
/// Script that implements an air tap gesture that will teleport the linked CodeBreaker to it's position
/// </summary>
[RequireComponent(typeof(Collider))]
public class ModelRelocator : MonoBehaviour, IInputClickHandler
{
    /*
     * Serializable
     */
    [Tooltip("The vertical offset to position the Code Breaker such that it is positioned centrally")]
    [SerializeField]
    private float verticalOffset = 0.05f;

    #region IInputClickHandler
    /// <summary>
    /// Handles air tap events
    /// </summary>
    /// <param name="eventData">Data concerning the air tap gesture</param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        // Teleport the Code Breaker here
        ModelLibrary.Instance.transform.position = transform.position;

        // Add offset
        ModelLibrary.Instance.transform.Translate(0.0f, verticalOffset, 0.0f);

        // If we're on the network
        if (NetworkPlayer.Instance != null)
        {
            // Introduction to Networked Experiences: Exercise 6.3
            /************************************************************/
            // Call NetworkPlayer's "CmdUpdateModelTransform()" and pass it the ModelLibrary's instance's local position and rotation
            
        }
    }
    #endregion
}