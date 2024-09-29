using UnityEngine;
using System;

public class Settings : MonoBehaviour
{
    public event Action OnDisableGun2; // disable the gun
    public event Action OnEnableGun; // enable the gun
    public static Settings Instance { get; private set; } // Singleton

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject closeGuidanceTextButton;

    public static event Action OnOpenSettingsPanel;

    private void Awake()
    {
        if (Instance != null) // Singleton
        {
            Destroy(gameObject);
            Debug.LogError("There can only be one Settings" + transform + " " + Instance);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnDisableGun2?.Invoke(); //disable the Gun scipt, call method DisableGun -> Gun.cs

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
        Cursor.visible = true;
        settingsPanel.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }

    public void CloseSettingsPanel()
    {
        Cursor.visible = false;       
        closeGuidanceTextButton.SetActive(true);
        Time.timeScale = 1; // Resume the game
        OnEnableGun?.Invoke(); //enable the Gun scipt, call method EnableGun -> Gun.cs
    }

    public void CallOnDisableGun2()
    {
        OnDisableGun2?.Invoke();
    }
}
