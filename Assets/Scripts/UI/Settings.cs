using UnityEngine;
using System;

public class Settings : MonoBehaviour
{
    public static Action OnDisableGun; //disable the gun
    public static Action OnEnableGun; //enable the gun
    public static Settings Instance { get; private set; }

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject closeGuidanceTextButton;

    public static event Action OnOpenSettingsPanel;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Debug.LogError("There can only be one Settings" + transform + " " + Instance);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnDisableGun?.Invoke(); //disable the Gun scipt, call method DisableGun -> Gun.cs
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
        OnEnableGun?.Invoke(); //enable the Gun scipt, call method EnableGun -> Gun.cs
        closeGuidanceTextButton.SetActive(true);
        Time.timeScale = 1; // Resume the game
    }
}
