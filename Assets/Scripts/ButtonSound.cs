using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonSound : MonoBehaviour
{
    [SerializeField] AudioClip _buttonSound;

    AudioSource source;

    private void Start() {
        source = GetComponent<AudioSource>();
        source.clip = _buttonSound;
    }

    public void Play(){
        source.Play();
    }
}
