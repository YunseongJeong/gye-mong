using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SoundType
{
    UI,
    EFFECT,
    FRIENDLY,
    ENEMY,
    BGM
}

public class SoundManager : SingletonObject<SoundManager>
{
    private const float DEFAULT_VOLUME = 1;

    private SoundObject bgmSoundObject;

    public SoundSourceList soundSourceList;
    private Dictionary<SoundType, float> volumes = new Dictionary<SoundType, float>();
    
    private void InitializeVolumes()
    {
        foreach (SoundType type in Enum.GetValues(typeof(SoundType))){
            volumes[type] = DEFAULT_VOLUME;
        }
    }

    public SoundObject GetBgmObject()
    {
        if (bgmSoundObject == null)
        {
            bgmSoundObject = gameObject.AddComponent<SoundObject>();
        }
        
        return bgmSoundObject;
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeVolumes();
    }

    public void SetVolume(SoundType type, float volume)
    {
        volumes[type] = volume;
    }

    public float GetVolume(SoundType type)
    {
        return volumes[type];
    }
}