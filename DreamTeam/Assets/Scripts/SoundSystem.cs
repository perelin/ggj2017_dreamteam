using UnityEngine;
using System.Collections;

    public class SoundSystem : MonoBehaviour 
    {
        public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
        public AudioSource[] musicSource;                 //Drag a reference to the audio source which will play the music.
        public static SoundSystem instance = null;     //Allows other scripts to call functions from SoundManager.             
        public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
        public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.
	public AudioClip[] musicList;

	// music transition stuff
	public float transitionLength = 1f; //how long should the transition take (in seconds)
	
	private int mainMusicSource = 0;
	private int currentMusicId = 0;
	private bool inTransition = false;
	private float transtitionStartTime;
        
        void Awake ()
        {
            //Check if there is already an instance of SoundManager
            if (instance == null)
                //if not, set it to this.
                instance = this;
            //If instance already exists:
            else if (instance != this)
                //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundSystem.
                Destroy (gameObject);
            
            //Set SoundSystem to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad (gameObject);
        }
        
        
        //Used to play single sound clips.
        public void PlaySingle(AudioClip clip)
        {
            //Set the clip of our efxSource audio source to the clip passed in as a parameter.
            efxSource.clip = clip;
            
            //Play the clip.
            efxSource.Play ();
        }

	public void ChangeMusic(int newId) {
	    if (newId != currentMusicId) {
		// swap music source pointers
		musicSource[1-mainMusicSource].clip = musicList[newId];
		musicSource[1-mainMusicSource].volume = 0f;
		musicSource[1-mainMusicSource].Play();
		currentMusicId = newId;
		// start transition
		transtitionStartTime = Time.time;
		inTransition = true;
	    }
	}
	
	
	public void Update() {
	    if (inTransition) {
		float progress = (Time.time - transtitionStartTime) / transitionLength;
		if (progress > 1f) {
		    // end transition
	   	    musicSource[mainMusicSource].Stop();
		    mainMusicSource = 1 - mainMusicSource;
		    inTransition = false;
		} else {
		    musicSource[mainMusicSource].volume = 1 - progress;
		    musicSource[1-mainMusicSource].volume = progress;
		}
	    }

	    // for debugging purposes only
	    if (Input.GetKey(KeyCode.Alpha1)) {
		ChangeMusic(0);
	    }
	    if (Input.GetKey(KeyCode.Alpha2)) {
		ChangeMusic(1);
	    }
	    if (Input.GetKey(KeyCode.Alpha3)) {
		ChangeMusic(2);
	    }
	}
        
        
        //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
        public void RandomizeSfx (params AudioClip[] clips)
        {
            //Generate a random number between 0 and the length of our array of clips passed in.
            int randomIndex = Random.Range(0, clips.Length);
            
            //Choose a random pitch to play back our clip at between our high and low pitch ranges.
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);
            
            //Set the pitch of the audio source to the randomly chosen pitch.
            efxSource.pitch = randomPitch;
            
            //Set the clip to the clip at our randomly chosen index.
            efxSource.clip = clips[randomIndex];
            
            //Play the clip.
            efxSource.Play();
        }
    }
