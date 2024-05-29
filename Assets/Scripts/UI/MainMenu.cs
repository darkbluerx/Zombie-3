using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    [SerializeField] GameObject mapSelectionCanvas;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There can only be one MainMenu" + transform + " " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        LevelSelectionMenu.OnShowMapSelectionCanvas += ShowCanvas;
        //Debug.Log("Canvas eventtiä kutsuttu");
    }

    private void ShowCanvas()
    {
        if (mapSelectionCanvas != null) mapSelectionCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
