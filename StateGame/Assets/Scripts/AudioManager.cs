using UnityEngine;

[System.Serializable()] //can view in inspector
public struct SoundParameters //contain values like volume, pitch, or loop the sound
{
    [Range(0, 1)]
    public float Volume;
    [Range(-3, 3)]
    public float Pitch;
    public bool Loop;

}

[System.Serializable()] //can view in inspector
public class Sound
{
    [SerializeField] string name;
    public string Name { get { return name; } }

    [SerializeField] AudioClip clip;
    public AudioClip Clip { get { return clip; } }

    [SerializeField] SoundParameters parameters;
    public SoundParameters Parameters { get { return parameters; } }

    [HideInInspector]
    public AudioSource Source; //reference to the audio source that we will instantiate

    public void Play() //play the sound
    {
        Source.clip = Clip;

        Source.volume = Parameters.Volume;
        Source.pitch = Parameters.Pitch;
        Source.loop = Parameters.Loop;

        Source.Play();
    }

    public void Stop() //stop the sound
    {
        Source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // have an instance of itself

    [SerializeField] Sound[] sounds;
    [SerializeField] AudioSource sourcePreFab;

    [SerializeField] string startupTrack;

    void Awake() //called before start function
    {
        if (Instance != null)
        {
            Destroy(gameObject);//destory audiomanager if it already exists
        }
        else
        {
            Instance = this; //instance will be this audiomanager
            DontDestroyOnLoad(gameObject); //if we change scenes or restore, we restore audio
        }
        InitSounds();
    }

    void Start()
    {
        if (string.IsNullOrEmpty(startupTrack) != true) //check if null
        {
            PlaySound(startupTrack);
        }
    }

    void InitSounds() //initiates sounds
    {
        foreach (var sound in sounds)
        {
            AudioSource source = (AudioSource)Instantiate(sourcePreFab, gameObject.transform); //parent is gameObject
            source.name = sound.Name;

            sound.Source = source;
        }
    }

    public void PlaySound(string name)
    {
        var sound = GetSound(name);
        if (sound != null)
        {
            sound.Play(); //if sound is not null, play the sound
        }
        else
        {
            Debug.LogWarning("Sound by the name " + name + " is not found at AudioManager.PlaySound");
        }
    }
    public void StopSound(string name)
    {
        var sound = GetSound(name);
        if (sound != null)
        {
            sound.Stop(); //if there is a sound, stop the sound
        }
        else
        {
            Debug.LogWarning("Sound by the name " + name + " is not found at AudioManager.StopSound");
        }
    }

    Sound GetSound(string name) //loop through all of the sounds and check if any match to the parameter
    {
        foreach (var sound in sounds)
        {
            if (sound.Name == name)
            {
                return sound;
            }
        }
        return null;
    }
}