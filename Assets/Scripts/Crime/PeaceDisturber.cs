using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scorable))]
[RequireComponent(typeof(Crime))]
[RequireComponent(typeof(AudioSource))]
public class PeaceDisturber : MonoBehaviour, I_Interactable
{
    [Tooltip("Any peace disturber can have multiple, randomized potential sound clips. If you only want one sound, leave one in array.")]
    public AudioClip[] potentialSoundClips;

    Scorable scoringComponent;
    Crime crimeComponent;
    AudioSource audioComponent;

    string descriptionText = "Potentially unused for now.";
    public string DescriptionText {  get { return descriptionText; } }

    // Start is called before the first frame update
    void Start()
    {
        scoringComponent = GetComponent<Scorable>();
        crimeComponent = GetComponent<Crime>();
        audioComponent = GetComponent<AudioSource>();
    }

    public void Interacted(GameObject caller = null)
    {
        scoringComponent.Score();
        crimeComponent.CommitCrime();

        if(potentialSoundClips.Length == 0)
        {
            Debug.LogWarning("PeaceDisturber object \"" + gameObject.name + "\" currently has no sound clips attached.");
        }
        else
        {
            int soundClipToPlay = Random.Range(0, potentialSoundClips.Length);
            audioComponent.clip = potentialSoundClips[soundClipToPlay];
            audioComponent.Play();
        }
    }

    public void BroadcastDescriptionText()
    {
        // Unused for now
    }
}
