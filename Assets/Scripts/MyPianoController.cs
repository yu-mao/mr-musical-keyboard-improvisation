using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PianoScale {
    Default = 0,
    Chinese = 1,
    Japanese = 2
}

public class MyPianoController : MonoBehaviour
{
    public List<AudioClip> keySounds;

    [SerializeField] private GameObject defaultPiano;
    [SerializeField] private GameObject chinesePiano;
    [SerializeField] private GameObject japanesePiano;

    public void RefreshPiano(PianoScale scale)
    {
        StartCoroutine(actualRefresh(scale));
    }

    private IEnumerator actualRefresh(PianoScale scale)
    {
        MuteAllScales();
        yield return new WaitForSeconds(0.2f);
        SetupNewScale(scale);
    }
    
    private void MuteAllScales()
    {
        defaultPiano.SetActive(false);
        chinesePiano.SetActive(false);
        japanesePiano.SetActive(false);
    }
    
    private void SetupNewScale(PianoScale scale)
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

    // private void Start()
    // {
    //     defaultPiano.SetActive(false);
    //     chinesePiano.SetActive(false);
    //     japanesePiano.SetActive(false);
    // }
}
