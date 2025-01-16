using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PianoScale {
    Default = 0,
    Chinese = 1,
    Japanese = 2
}

// public class MyPianoKey
// {
//     public GameObject key { get; set; }
//     public Transform originalKeyPosition { get; set; }
//     public bool isPressed { get; set; }
// }

public class MyPianoController : MonoBehaviour
{
    public List<AudioClip> keySounds;

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

    public void InitializeScale(PianoScale scale)
    {
        switch (scale)
        {
            case PianoScale.Default:
                defaultPiano.SetActive(true);
                break;
            case PianoScale.Chinese:
                chinesePiano.SetActive(true);
                break;
            case PianoScale.Japanese:
                japanesePiano.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        defaultPiano.SetActive(false);
        chinesePiano.SetActive(false);
        japanesePiano.SetActive(false);
    }
}
