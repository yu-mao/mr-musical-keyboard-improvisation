using System;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private FindSpawnPositions _findSpawnPositions;
    
    private MyPianoController _pianoController;

    public void Initialize()
    {
        
        _findSpawnPositions.StartSpawn();
        _pianoController = _findSpawnPositions.GetComponentInChildren<MyPianoController>();
        Debug.Log("Found piano: " + _pianoController);
        
        var _room = MRUK.Instance.GetCurrentRoom();

        MRUKAnchor anchor = new MRUKAnchor();
        anchor.
    }
}
