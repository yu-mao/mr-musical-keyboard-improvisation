using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private TestHandCollider _handCollider;
    [SerializeField] private GameObject _pianoPrefab;
    [SerializeField] private FindSpawnPositions _findSpawnPositions;

    
    private GameObject _roomGameObject;
    private MyPianoController _pianoController;
    private bool _canSpawnPiano = true;
    private Vector3 _collisionPosition;

    public void Initialize()
    {
        _roomGameObject = FindAnyObjectByType<MRUKRoom>().gameObject;
        ApplyLayer(_roomGameObject, "Room");
    }

    private void SpawnPiano(Vector3 position)
    {
        // align piano placement direction with hand orientation on Y axis
        Vector3 eulerAngles = _handCollider.transform.rotation.eulerAngles;
        eulerAngles.x = 0;
        eulerAngles.z = 0;
        Quaternion projectedRotation = Quaternion.Euler(eulerAngles);
        
        GameObject piano = Instantiate(_pianoPrefab, position, projectedRotation);
    }

    private void Update()
    {
        if (_canSpawnPiano && _handCollider.hasCollided)
        {
            SpawnPiano(_handCollider.collisionPosition);
            StartCoroutine(SpawnCooldown());
        }
    }

    private IEnumerator SpawnCooldown()
    {
        _canSpawnPiano = false;
        yield return new WaitForSeconds(0.5f);
        _canSpawnPiano = true;
    }

    private void ApplyLayer(GameObject obj, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        obj.layer = layer;
        
        foreach(Transform child in obj.transform) ApplyLayer(child.gameObject, layerName);
    }
}
