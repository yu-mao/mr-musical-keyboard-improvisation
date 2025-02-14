using System;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private TestHandCollider _handCollider;
    [SerializeField] private GameObject _pianoPrefab;
    [SerializeField] private FindSpawnPositions _findSpawnPositions;
    [SerializeField] private GameObject _musicEffectChinese;
    [SerializeField] private GameObject _musicEffectJapanese;
    
    private GameObject _roomGameObject;
    private bool _canSpawnPiano = false;
    private List<GameObject> _spawnedPianos = new List<GameObject>();
    private MyPianoController _currentPiano;
    private PianoScale _currentScale = PianoScale.Chinese;
    
    public void Initialize()
    {
        _roomGameObject = FindAnyObjectByType<MRUKRoom>().gameObject;
        ApplyLayer(_roomGameObject, "Room");
        
        // // for testing in Unity editor
        // SpawnPiano(transform.position);
        // SwitchScale();
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

    public void SwitchScale()
    {
        if (_currentPiano == null) return;
        
        Array scales = Enum.GetValues(typeof(PianoScale));
        int currentIndex = Array.IndexOf(scales, _currentScale);
        int nextIndex = (currentIndex + 1) % scales.Length; // Compute the next index (wrap around if necessary)
        _currentScale = (PianoScale)nextIndex; // cast the index as a specific scale in PianoScale enum
        
        Refresh();
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
        _spawnedPianos.Add(piano);
        
        _currentPiano = piano.GetComponent<MyPianoController>();
        Refresh();
    }

    private void Refresh()
    {
        _currentPiano.Refresh(_currentScale);
        // RemoveMusicEffect();
        // SpawnMusicEffect();
    }

    // private void RemoveMusicEffect()
    // {
    //     GameObject musicEffect = FindAnyObjectByType<MusicEffectController>().gameObject;
    //     Destroy(musicEffect);
    // }

    private void SpawnMusicEffect()
    {
        switch (_currentScale)
        {
            case PianoScale.Default:
                break;
            case PianoScale.Chinese:
                _findSpawnPositions.SpawnObject = _musicEffectChinese;
                _findSpawnPositions.StartSpawn();
                break;
            case PianoScale.Japanese:
                _findSpawnPositions.SpawnObject = _musicEffectJapanese;
                _findSpawnPositions.StartSpawn();
                break;
            default:
                break;
        }
    }
}
