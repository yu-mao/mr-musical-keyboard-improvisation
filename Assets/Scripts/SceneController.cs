using System;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private FindSpawnPositions _findSpawnPositions;

    private GameObject _roomGameObject;
    private MyPianoController _pianoController;
    

    public void Initialize()
    {
        _roomGameObject = FindAnyObjectByType<MRUKRoom>().gameObject;
        // Debug.Log("MRUK Room: " + _roomGameObject.name);
        
        SpawnPiano();
    }
    
    private void SpawnPiano()
    {
        _findSpawnPositions.StartSpawn();
        _pianoController = _findSpawnPositions.GetComponentInChildren<MyPianoController>();
        // Debug.Log("Found piano: " + _pianoController);
    }
}
