using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace ABDeveloper.PianoController
{
  public partial class PianoKey
  {
    /// <summary>
    /// Unity GameObject of piano key
    /// </summary>
    public GameObject pianoKey { get; set; }

    /// <summary>
    /// Hold reference to original position of the key 
    /// </summary>
    public Transform originalPianoKeyPosition { get; set; }

    /// <summary>
    /// KeyCode reference so we can easily find out which keys are still pressed
    /// </summary>
    public KeyCode keyboardKeyName { get; set; }

    /// <summary>
    /// Indicate if the key is currently pressed
    /// </summary>
    public bool isPressed { get; set; }
  }

  /// <summary>
  /// Piano controller that has to be attached to Piano. 
  /// Developed by Adam Bielecki
  /// Contact : adam@visualab.co.uk or theadambielecki@gmail.com
  /// </summary>
  public class PianoController : MonoBehaviour
  {
    public AudioClip[] keySounds; // Assign your key sounds in the Unity Editor

    List<PianoKey> pianoKeys = new List<PianoKey>();

    private bool canPressButton = true;
    private float buttonPressCooldown = 0.025f;

    private const int totalOctaves = 7;

    // Current octave
    private int currentOctave = 3;

    private readonly Dictionary<string, string> keyMapping = new Dictionary<string, string>
    {
        {"s", "C"},
        {"e", "C-sharp"},
        {"d", "D"},
        {"r", "D-sharp"},
        {"f", "E"},
        {"g", "F"},
        {"y", "F-sharp"},
        {"h", "G"},
        {"u", "G-sharp"},
        {"j", "A"},
        {"i", "A-sharp"},
        {"k", "B"}
    };

    // Start is called before the first frame update
    void Start()
    {
      var allKeys = GameObject.FindGameObjectsWithTag("PianoKey");

      for (int i = 0; i < allKeys.Length; i++)
      {
        pianoKeys.Add(new PianoKey { originalPianoKeyPosition = allKeys[i].transform, pianoKey = allKeys[i] });
      }
    }

    void MapKeyboardInput()
    {
      // Check if any key is pressed
      if (Input.anyKeyDown && canPressButton)
      {
        // Check if a numeric key (1-7) is pressed to change the current octave
        int octaveChange;
        if (int.TryParse(Input.inputString, out octaveChange) && octaveChange >= 1 && octaveChange <= totalOctaves)
        {
          currentOctave = octaveChange;
          Debug.Log("Current Octave: " + currentOctave);
        }
        else
        {
          // Check if the pressed key is in the dictionary
          string pressedKey;
          if (keyMapping.TryGetValue(Input.inputString, out pressedKey))
          {
            // Create the full key name using the pressed key and current octave (e.g., Key_C3)

            string fullKeyName = string.Empty;

            Debug.Log("Pressed keyboard key " + Input.inputString);

            if (pressedKey.Contains("sharp"))
            {
              var splitHash = pressedKey.Split("-");
              fullKeyName = "Key_" + splitHash[0] + currentOctave + "#";
            }
            else
            {
              fullKeyName = "Key_" + pressedKey + currentOctave;
            }

            Debug.Log("Pressed keyboard key (full name of game object) " + fullKeyName);

            // Trigger the corresponding action or function with the full key name

            var currentlyPressedPianoKey = pianoKeys?.Find(m => m.pianoKey?.name == fullKeyName);
            if (!currentlyPressedPianoKey.isPressed)
            {
              // Play corresponding sound
              PlaySound(fullKeyName);

              AnimateKey(currentlyPressedPianoKey.pianoKey, true);


              currentlyPressedPianoKey.isPressed = true;

              KeyCode pressedKeyboardKey = (KeyCode)Enum.Parse(typeof(KeyCode), Input.inputString, true);
              currentlyPressedPianoKey.keyboardKeyName = pressedKeyboardKey;
            }
          }
        }
      }
    }

    void PlaySound(string keyName)
    {
      // Find the index of the key in the array
      int keyIndex = -1;
      for (int i = 0; i < keySounds.Length; i++)
      {
        if (keySounds[i] != null && keySounds[i].name == keyName)
        {
          keyIndex = i;
          break;
        }
      }

      // Play the sound if found, otherwise play the default sound
      if (keyIndex != -1)
      {
        AudioSource.PlayClipAtPoint(keySounds[keyIndex], transform.position);
      }
    }

    // Update is called once per frame
    void Update()
    {
      MapKeyboardInput();

      // Check for mouse click
      if (canPressButton && Input.GetMouseButton(0))
      {
        // Cast a ray from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits an object
        if (Physics.Raycast(ray, out hit))
        {
          // Get the name of the object
          string objectName = hit.collider.gameObject.name;

          var currentKey = pianoKeys?.Find(m => m.pianoKey?.name == objectName);

          if (!currentKey.isPressed)
          {
            // Play corresponding sound
            PlaySound(objectName);

            AnimateKey(hit.collider.gameObject, true);

            currentKey.isPressed = true;
          }
        }

        StartCoroutine(ButtonCooldown());

      }
      else if (canPressButton && !Input.GetMouseButton(0))
      {
        ResetNonPressedKeys();
      }
    }

    void ResetNonPressedKeys()
    {
      // no activity, move key up
      foreach (var k in pianoKeys.Where(k => k.isPressed))
      {
        if (k.isPressed)
        {
          // check if keyboard key is pressed, otherwise do not remove

          if (!Input.GetKey(k.keyboardKeyName))
          {
            // we do not want to 
            Debug.Log(k.pianoKey.name);
            AnimateKey(k.pianoKey, false);

            // make sure to go back to original position, just in case animation did not work properly
            k.pianoKey.transform.position = k.originalPianoKeyPosition.position;

            k.isPressed = false;

            Debug.Log($"Set key press status of {k.pianoKey} to {k.isPressed}");
          }
        }
      }
    }

    IEnumerator ButtonCooldown()
    {
      // Set the cooldown flag to false
      canPressButton = false;

      // Wait for the specified cooldown duration
      yield return new WaitForSeconds(buttonPressCooldown);

      // Set the cooldown flag to true, allowing the button to be pressed again
      canPressButton = true;
    }


    IEnumerator AnimateKeyCoroutine(GameObject gameObject, bool keyIsPressed)
    {
      Debug.Log("Animating key: " + gameObject.name + ", Pressed: " + keyIsPressed);

      Animator keyAnimator = gameObject.GetComponent<Animator>();

      Debug.Log(keyAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash);

      keyAnimator.SetBool("KeyIsPressed", keyIsPressed);
      Debug.Log("length " + keyAnimator.GetCurrentAnimatorStateInfo(0).length);
      yield return new WaitForSeconds(keyAnimator.GetCurrentAnimatorStateInfo(0).length);

      Debug.Log("Animation complete for key: " + gameObject.name);
    }

    void AnimateKey(GameObject gameObject, bool keyIsPressed)
    {
      StartCoroutine(AnimateKeyCoroutine(gameObject, keyIsPressed));
    }
  }
}