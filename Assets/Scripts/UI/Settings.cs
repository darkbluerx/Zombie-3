using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject closeGuidanceTextButton;

    public static event Action OnOpenSettingsPanel;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnOpenSettingsPanel?.Invoke();
        }
    }

    private void OnEnable()
    {
      OnOpenSettingsPanel += OpenSettingsPanel;    
    }

    public void OpenSettingsPanel()
    {
        if (settingsPanel == null) { return; }
        if (closeGuidanceTextButton == null) { return; }

        closeGuidanceTextButton.SetActive(false);
        //Cursor.lockState = CursorLockMode.None;
        settingsPanel.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }

    public void CloseSettingsPanel()
    {
        closeGuidanceTextButton.SetActive(true);
        //Cursor.lockState = CursorLockMode.Locked;
        //settingsPanel.SetActive(false);
        Time.timeScale = 1; // Resume the game
    }

}
