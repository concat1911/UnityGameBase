namespace EM.Sound
{
    using UnityEngine;
	using UnityEngine.Pool;
	using UnityEngine.Audio;
    using System.Collections.Generic;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using DG.Tweening;

    public class SoundManager : MonoBehaviour
    {
        private static SoundManager instance;

        [SerializeField] bool muted = false;
	    [SerializeField] GameObject controllerPrefab;

        [Header("Background Music")]
        [SerializeField] private AudioSource bgMusicAudioSrc;
        [SerializeField] private float bgMusicVolume = 0.5f;
	    [SerializeField] private float bgMusicFadeDuration = 0.5f;

	    [Header("AudioMixers")]
	    [SerializeField] private AudioMixerGroup masterMixer;
	    [SerializeField] private AudioMixerGroup musicMixer;
	    [SerializeField] private AudioMixerGroup vfxMixer;
	    [SerializeField] private AudioMixerGroup defaultMixer;
	    
	    public AudioMixerGroup MasterMixer => masterMixer;
	    public AudioMixerGroup MusicMixer => musicMixer;
	    public AudioMixerGroup VFXMixer => vfxMixer;
	    public AudioMixerGroup DefaultMixer => defaultMixer;
	    
        public static bool Muted => instance.muted;

        [SerializeField] List<SoundController> activeSound = new List<SoundController>();

        IObjectPool<SoundController> soundPool;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            bgMusicAudioSrc = GetComponent<AudioSource>();
            soundPool = new ObjectPool<SoundController>(CreateSoundController, OnTakeSoundController, OnReleaseSoundController);
        }

        private void Update()
        {
            for(int i = 0; i < activeSound.Count; i++)
            {
                if( activeSound[i].IsFinished() )
                {
                    soundPool.Release(activeSound[i]);
                    activeSound.RemoveAt(i);
                    i--;
                }
            }
        }

        SoundController CreateSoundController()
        {
            GameObject controllerObj = Instantiate(controllerPrefab, transform);
            SoundController controller = controllerObj.GetComponent<SoundController>();

            return controller;
        }

        void OnTakeSoundController(SoundController soundController)
        {
            soundController.gameObject.SetActive(true);
        }

        void OnReleaseSoundController(SoundController soundController)
        {
            soundController.gameObject.SetActive(false);
        }

	    public static bool Play3D(AudioClip clip, Vector3 pos, float volume = 1f, float pitch = 1f, SoundType soundType = SoundType.Default)
        {
            if (instance.muted) return false;
            if (clip == null) return false;

            SoundController controller = instance.soundPool.Get();
            controller.transform.position = pos;
	        controller.Play3D(clip, volume, pitch, soundType);

            instance.activeSound.Add(controller);

            return true;
        }

        public static bool Play2D(AudioClip clip, float volume = 1f, float pitch = 1f, SoundType soundType = SoundType.Default)
        {
            if (instance.muted) return false;
            if( clip == null ) return false;

	        SoundController controller = instance.soundPool.Get();
            
	        controller.SetAudioMixerGroup(instance.GetAudioMixer(soundType));
	        controller.Play2D(clip, volume, pitch, soundType);

            instance.activeSound.Add(controller);

            return true;
        }

        public static SoundController GetSoundController()
        {
            return instance.soundPool.Get();
        }

        public static void ReturnSoundController(SoundController controller)
        {
            instance.soundPool.Release(controller);
        }

        public static bool SoundToggle()
        {
            instance.muted = !instance.muted;

            if(instance.muted)
            {
                BGMusicPause();
            }
            else
            {
                BGMusicResume();
            }

            return instance.muted;
        }
        
        public static void SetSoundStatus(bool status)
        {
            instance.muted = status;

            if (instance.muted)
            {
                BGMusicPause();
            }
            else
            {
                BGMusicResume();
            }
        }

        public static void BGMusicPause()
        {
            instance.bgMusicAudioSrc.DOFade(0f, instance.bgMusicFadeDuration)
                .OnComplete(() => {
                    instance.bgMusicAudioSrc.Pause();
                });
        }

        public static void BGMusicResume()
        {
            if( instance.muted )
            {
                BGMusicPause();
                return;
            }

            instance.bgMusicAudioSrc.DOFade(instance.bgMusicVolume, instance.bgMusicFadeDuration)
                .OnComplete(() => {
                    instance.bgMusicAudioSrc.Play();
                });
        }

        public static void ChangeBGMusic(string assetName)
        {
            if (instance.muted) return;

            Addressables.LoadAssetAsync<AudioClip>(assetName).Completed += instance.OnNewBGMusicLoaded;
        }

        void OnNewBGMusicLoaded(AsyncOperationHandle<AudioClip> handle)
        {
            AudioClip newBGMusic = handle.Result;

            bgMusicAudioSrc.DOFade(0f, bgMusicFadeDuration)
                .OnComplete(() => {
                    bgMusicAudioSrc.clip = newBGMusic;
                    bgMusicAudioSrc.Play();
                    bgMusicAudioSrc.DOFade(bgMusicVolume, bgMusicFadeDuration);
                });
        }

	    AudioMixerGroup GetAudioMixer(SoundType soundType)
	    {
	    	switch(soundType){
	    		
	    	default:
	    	case SoundType.Default:
		    	return defaultMixer;
	    	case SoundType.VFX:
		    	return vfxMixer;
	    	case	SoundType.BGMusic:
		    	return musicMixer;
	    	}
	    }
    }
}
