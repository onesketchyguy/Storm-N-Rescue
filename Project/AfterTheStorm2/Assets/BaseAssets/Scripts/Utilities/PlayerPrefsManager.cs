using UnityEngine;

public class PlayerPrefsManager
{
    private const string SFX_Volume = "SFX_VOLUME";

    private const string Music_Volume = "MUSIC_VOLUME";

    private const string Master_Volume = "MASTER_VOLUME";

    public static void ResetValues()
    {
        MasterVolume = 1;
        MusicVolume = 0.5f;
        SFXVolume = 1;
    }

    public static int GoldCount
    {
        get
        {
            return PlayerPrefs.GetInt("GOLD_COUNT", 0);
        }
        set
        {
            PlayerPrefs.SetInt("GOLD_COUNT", value);
        }
    }

    public static float MasterVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(Master_Volume);
        }
        set
        {
            PlayerPrefs.SetFloat(Master_Volume, value);
        }
    }

    public static float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(Music_Volume) * MasterVolume;
        }
        set
        {
            PlayerPrefs.SetFloat(Music_Volume, value);
        }
    }

    public static float SFXVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(SFX_Volume) * MasterVolume;
        }
        set
        {
            PlayerPrefs.SetFloat(SFX_Volume, value);
        }
    }

    public static bool FirstLaunch
    {
        get
        {
            return PlayerPrefs.GetInt("FIRST_LAUNCH") != 1;
        }
        set
        {
            if (value == false)
            {
                PlayerPrefs.SetInt("FIRST_LAUNCH", 1);
            }
            else
            {
                PlayerPrefs.SetInt("FIRST_LAUNCH", 0);
            }
        }
    }
}