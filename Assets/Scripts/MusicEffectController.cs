using System;
using UnityEngine;

public class MusicEffectController : MonoBehaviour
{
    // rotation speed in degrees per second
    [SerializeField] private Vector3 _rotationSpeed = new Vector3(0, 10, 0); 
    
    private void Update()
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime);
    }
}
