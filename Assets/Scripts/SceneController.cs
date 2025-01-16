using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private TestHandCollider _handCollider;
    [SerializeField] private GameObject _pianoPrefab;
    // [SerializeField] private FindSpawnPositions _findSpawnPositions;
    
    private GameObject _roomGameObject;
    private bool _canSpawnPiano = false;
    private Vector3 _collisionPosition;
    private List<GameObject> _spawnedPianos = new List<GameObject>();
    
    public void Initialize()
    {
        _roomGameObject = FindAnyObjectByType<MRUKRoom>().gameObject;
        ApplyLayer(_roomGameObject, "Room");
        // _findSpawnPositions.StartSpawn();
        // SpawnPiano(transform.position); // for testing in Unity editor
    }
    
    private void ApplyLayer(GameObject obj, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        obj.layer = layer;
        
        foreach(Transform child in obj.transform) ApplyLayer(child.gameObject, layerName);
    }
    
    public void RecreateKeyboard()
    {
        if (!_canSpawnPiano) _canSpawnPiano = true;

        if (_spawnedPianos.Count >= 1)
        {
            Destroy(_spawnedPianos[_spawnedPianos.Count - 1]);
            _spawnedPianos.RemoveAt(_spawnedPianos.Count - 1);
        }
    }

    private void Update()
    {
        if (_canSpawnPiano && _handCollider.hasCollided)
        {
            SpawnPiano(_handCollider.collisionPosition);
            _canSpawnPiano = false;
        }
    }
    
    private void SpawnPiano(Vector3 position)
    {
        // align piano placement direction with hand orientation and project to XZ plane
        Vector3 eulerAngles = _handCollider.transform.rotation.eulerAngles;
        eulerAngles.x = 0;
        eulerAngles.z = 0;
        Quaternion projectedRotation = Quaternion.Euler(eulerAngles);

        var piano = Instantiate(_pianoPrefab, position, projectedRotation);
        StartCoroutine(InitializePiano(piano));
    }

    private IEnumerator InitializePiano(GameObject piano)
    {
        yield return new WaitForSeconds(0.2f);
        var pianoController = piano.GetComponent<MyPianoController>();
        pianoController.InitializeScale(PianoScale.Japanese);
        _spawnedPianos.Add(piano);
    }
}
