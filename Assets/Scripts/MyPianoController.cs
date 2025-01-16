using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// public class MyPianoKey
// {
//     public GameObject key { get; set; }
//     public Transform originalKeyPosition { get; set; }
//     public bool isPressed { get; set; }
// }

public class MyPianoController : MonoBehaviour
{
    public List<AudioClip> keySounds;
    public PianoScale scale = PianoScale.Chinese;

    [SerializeField] private GameObject defaultPiano;
    [SerializeField] private GameObject chinesePiano;
    [SerializeField] private GameObject japanesePiano;

    // private List<MyPianoKey> _pianoKeys = new List<MyPianoKey>();

    // private void Start()
    // {
    //     var allKeys = GameObject.FindGameObjectsWithTag("PianoKey");
    //     for (int i = 0; i < allKeys.Length; i++)
    //     {
    //         _pianoKeys.Add(new MyPianoKey
    //         {
    //             originalKeyPosition = allKeys[i].transform,  
    //             key = allKeys[i], 
    //             isPressed = false
    //         });
    //     }
    // }

    private void Start()
    {
        
    }
}
