using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyPianoKey
{
    public GameObject key { get; set; }
    public Transform originalKeyPosition { get; set; }
    public bool isPressed { get; set; }
}

public class MyPianoController : MonoBehaviour
{
    private List<MyPianoKey> _pianoKeys = new List<MyPianoKey>();
    private bool _canPressKey = true;
    private float _keyPressCooldownTime = 0.025f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        var allKeys = GameObject.FindGameObjectsWithTag("PianoKey");
        for (int i = 0; i < allKeys.Length; i++)
        {
            _pianoKeys.Add(new MyPianoKey
            {
                originalKeyPosition = allKeys[i].transform,  
                key = allKeys[i], 
                isPressed = false
            });
        }
    }

    private void Update()
    {
        // if (_canPressKey)
    }


}
