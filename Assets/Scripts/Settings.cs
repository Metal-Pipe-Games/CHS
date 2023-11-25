using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject GameplayMenu;
    public GameObject GraphicsMenu;
    public GameObject ControlMenu;
    public GameObject AudioMenu;
    public GameObject AccessabilityMenu;

    public static float Volume = 1;


    public SettingTypes currentMenu = SettingTypes.Gameplay;

    public bool freezeTime = true;
    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = Volume;
    }

    private void OnEnable()
    {
        currentMenu = SettingTypes.Gameplay;
        OpenMenu(0);
        if(freezeTime)Time.timeScale = 0;
    }

    private void OnDisable()
    {
        if(freezeTime)Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu(int i)
    {
        var menu = (SettingTypes)i;
        switch (menu)
        {
            case SettingTypes.Gameplay:
                GraphicsMenu.SetActive(false); ControlMenu.SetActive(false); AudioMenu.SetActive(false); AccessabilityMenu.SetActive(false);
                GameplayMenu.SetActive(true);
                break;
            case SettingTypes.Graphics:
                GameplayMenu.SetActive(false); ControlMenu.SetActive(false); AudioMenu.SetActive(false); AccessabilityMenu.SetActive(false);
                GraphicsMenu.SetActive(true);
                break;
            case SettingTypes.Controls:
                GameplayMenu.SetActive(false); GraphicsMenu.SetActive(false); AudioMenu.SetActive(false); AccessabilityMenu.SetActive(false);
                ControlMenu.SetActive(true);
                break;
            case SettingTypes.Audio:
                GameplayMenu.SetActive(false); ControlMenu.SetActive(false); GraphicsMenu.SetActive(false); AccessabilityMenu.SetActive(false);
                AudioMenu.SetActive(true);
                break;
            case SettingTypes.Accessability:
                GameplayMenu.SetActive(false); GraphicsMenu.SetActive(false); GraphicsMenu.SetActive(false); AudioMenu.SetActive(false);
                AccessabilityMenu.SetActive(true);
                break;
        }
    }
}
public enum SettingTypes {
    Gameplay,
    Graphics,
    Controls,
    Audio,
    Accessability
}