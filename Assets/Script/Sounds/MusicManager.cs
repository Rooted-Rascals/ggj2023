using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Script.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {

        private static MusicManager INSTANCE;

        public static MusicManager Instance
        {
            get
            {
                return INSTANCE;
            }
        }

        private List<AudioClip> _musics;
        private AudioSource _source;

        public void Awake()
        {
            if (INSTANCE == null)
            {
                INSTANCE = this;
            }
            else if (INSTANCE != this)
            {
                Destroy(this);
            }
            DontDestroyOnLoad(this);

            _musics = Resources.LoadAll<AudioClip>("Sounds/Music/").ToList();
            _source = GetComponent<AudioSource>();
        }

        public void Start()
        {
                StartCoroutine(nameof(PlayMusic));
        }
        
        IEnumerator PlayMusic()
        {
            yield return null;

            while (true)
            {
                ShuffleMusic();

                foreach (AudioClip song in _musics)
                {
                    _source.clip = song;
                    _source.Play();

                    while (_source.isPlaying)
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(4);
                }
            }
        }

        private void ShuffleMusic()
        {
            Random rand = new Random();
            
            int n = _musics.Count;  
            while (n > 1) {  
                n--;  
                int k = rand.Next(n + 1);  
                (_musics[k], _musics[n]) = (_musics[n], _musics[k]);
            }  
        }
    }
}