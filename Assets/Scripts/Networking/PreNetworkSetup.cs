using HoloToolkit.Unity.SharingWithUNET;
using UnityEngine;

/// <summary>
/// Script that executes some initialization before networking starts
/// </summary>
public class PreNetworkSetup : MonoBehaviour
{
	void Start ()
    {
        // Introduction to Networked Experiences: Exercise 4.4
        /************************************************************/
        // Deactivate the shared collection gameObject
        SharedCollection.Instance.gameObject.SetActive(false);

    }
}
