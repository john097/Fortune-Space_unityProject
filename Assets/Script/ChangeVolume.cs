using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ChangeVolume : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider audioSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        float f;
        audioMixer.GetFloat("Main",out f);
        audioSlider.value = f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMainVolume()
    {
        audioMixer.SetFloat("Main",audioSlider.value);
    }
}
