using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static PlayerShoot Instance { get; private set;} //Singleton

    public event Action OnShootInput;
    public event Action OnReloadInput;

    [Header("Choose your Inputs")]
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    private void Awake()
    {
        if(Instance!= null)
        {
            Debug.LogWarning("There is already an instance of PlayerShoot in the scene" + transform + " "+ Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {    
        ShootInput();
        ReloadInput();
    }

    private void ReloadInput()
    {
        if (Input.GetKeyDown(reloadKey)) OnReloadInput?.Invoke();
    }

    private void ShootInput()
    {
        if (Input.GetKeyDown(shootKey) || (Input.GetKey(shootKey))) OnShootInput?.Invoke();
    }
}
