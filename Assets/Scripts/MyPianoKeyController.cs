using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MyPianoKeyController : MonoBehaviour
{
    [SerializeField] private MyPianoController _pianoController;

    private AudioClip _sound;
    private Animator _animator;
    private float _keyPressCooldownTime = 0.02f;
    private bool _canInteract = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sound = _pianoController.keySounds.FirstOrDefault(clip => clip.name == gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_canInteract)
        {
            AnimateKey(true);
            PlaySound();
            StartCoroutine(KeyCooldown());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (_canInteract)
        {
            AnimateKey(false);
        }
    }
    
    private void PlaySound()
    {
        AudioSource.PlayClipAtPoint(_sound, transform.position);
    }
    
    private void AnimateKey(bool isPressed)
    {
        _animator.SetBool("KeyIsPressed", isPressed);
    }
    
    private IEnumerator KeyCooldown()
    {
        _canInteract = false;
        yield return new WaitForSeconds(_keyPressCooldownTime);
        _canInteract = true;
    }
}
