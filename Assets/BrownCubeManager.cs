using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownCubeManager : MonoBehaviour, IInputClickHandler
{

    #region Singleton
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static BrownCubeManager Instance { get; private set; }
    #endregion

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
            Debug.LogError("More than 1 BrownCubeManager instance detected! Deleting this new one.");
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

    public GameObject original;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (NetworkPlayer.Instance == null)
        {
            SpawnCube(); 
        }

        else
        {
            NetworkPlayer.Instance.CmdSpawnCube();
        }
    }

    public void Start()
    {
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    public void SpawnCube()
    {
        GameObject cube = GameObject.Instantiate(original);
        cube.transform.position = Camera.main.transform.TransformPoint(0, 0, 1.2f);
        Debug.Log("Airtap!");
    }
    public void Update()
    { 
    }

}