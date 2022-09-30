using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundBtn : MonoBehaviour
{
    [SerializeField] AudioMixer _gameMixer;
    [SerializeField] bool _isSoundOn = true;
    [SerializeField] Sprite _onButton;
    [SerializeField] Sprite _offButton;
    [SerializeField] Image _spriteRender;

    private void Start() {
        applyState(_isSoundOn);
    }

    public void ToggleSound(){
        _isSoundOn = !_isSoundOn;
        applyState(_isSoundOn);
    }

    void applyState(bool state){
        setSoundLevel(state);
        setImage(state);
    }

    void setSoundLevel(bool isOn){
        float value = isOn ? 0f : -80f;

        _gameMixer.SetFloat("GameSound",value);
    }

    void setImage(bool isOn){
        Sprite image = isOn ? _onButton : _offButton;

        _spriteRender.sprite = image;
    }


}
