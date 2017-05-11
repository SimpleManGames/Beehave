/*
 *  Purpose
 *      To demonstrate a decent example of what code might look like.
 *      These header comments won't be enforced but are preferred.
 *      They should contain the main purpose of the script if included as well as...
 * 
 *  Dependencies
 *      None
 *      
 *  Accessors
 *      None
 */

// Don't forget that you can use macros with a using statement.
// Please don't do this in the extremely stupid way I'm using in the example.
using exampleMacro = UnityEngine.GameObject;
using UnityEngine;

// For the sake of the example, let's say this is an audio class.
// If you require components but not scripts, it will suffice to use RequireComponent
[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioClip))]

public class Example : MonoBehaviour
{

    // Regions are preferred for a few reasons, but not enforced.
    // They keep code organized and force you to name the region.
    // Naming a region can help you think about the purpose of things.
    #region Variables

    // Variables should be camel case and descriptive. Exception will be shown.
    // Getters and setters are preferred but not required. Name them appropriately if used.
    // Strictly private variables should be noted, if named are repeated then use an underscore.
    // Add [SerializeField] to private variable to see in inspector

    // Sub regions should all be named proper if used.
    #region Components

    // The audio listener component on the object. Read-Only.
    private AudioListener _audioListener;
    public AudioListener audioListener
    { get { return _audioListener; } }

    // The sound to be played back from this object.
    private AudioClip _audioClip;
    public AudioClip audioClip
    { get { return _audioClip; } set { _audioClip = value; } }

    #endregion

    #region Modifiers

    // Controls the master volume of the audio listener.
    public float volume;

    // Controls how far the user can listen.
    public float distance;

    // Dictates the compression for the audio clip, 0 being low quality, 1 being large file.
    [Range(0, 1)]
    public float compressionRate;

    // The pitch adjustment for the buzz.
    public float minPitch;
    public float maxPitch;

    #endregion

    #endregion

    #region Functions

    // Function names should preferrably be upper case with few exceptions.
    // One line functions should look clean, example will be provided.
    // I would prefer braces on the same indentation level, but won't force it.

    #region Unity

    // I won't be too annoying about this, but with-in unity functions I would prefer if
    // we didn't have any actual code, but instead used all functions with-in it.
    // It's clean and forces you to make sensible functions.
    // In some cases scripts may be so simple you might want to have it all in start/update though.

    // On start grab the components required to run the script.
    void Start()
    { GetComponents(); }

    // Each frame do a regular buzz and an obnoxious buzz.
    void Update()
    {
        Buzz();
        BuzzMore(PitchShift(minPitch, maxPitch));
    }

    #endregion

    #region Behaviour

    // Play the buzz clip
    void Buzz() {
        // I will enforce safety checks further in production. Even if an error *can* be ignored, we shouldn't.
        // It still bogs down performance and it's bad practice.
        // Also, I appreciate proper debugging.
        // We only want debug stuff in editor, otherwise the else still takes up processing.
        if (audioClip != null) { /* default buzz */ }
#if UNITY_EDITOR
        else Debug.Log("Warning: audioClip in Example.cs null in Buzz function!");
#endif

    }

    // Pretend this plays audio or some shit
    void BuzzMore(float pitch)
    {
        if (audioClip != null) { /* use pitch */ }
#if UNITY_EDITOR
        else Debug.Log("Warning: audioClip in Example.cs null in BuzzMore function!");
#endif

    }

    #endregion

    #region Modulation

    // Returns the number to adjust pitch by.
    float PitchShift(float min, float max)
    { return Random.Range(min, max); }

    #endregion

    #region Set-up

    // Grab all the necessary components to run the script.
    void GetComponents()
    {
        _audioListener = GetComponent<AudioListener>();
        _audioClip = GetComponent<AudioClip>();
    }

    #endregion

    #endregion

}

/*
 *  End notes
 *      We should have as little as possible, or no repeat code at all.
 *      To achieve this we should use...
 *          Global helper methods (Perhaps for math functions we use often or whatever)
 *          Inheritance (If we have two bee classes, chances are they both move the same way. You can over ride behaviour to adjust it.)
 *          Documentation (Please read changes, don't make something someone else may have already done.
 *          
 *      This isn't as much of a list of rules as it is guide lines.
 *      I would very much like to see most of this followed, but it isn't essential.
 *      For anyone who strays very far from this, expect your code to be reformatted.
 */