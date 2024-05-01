using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    Image crosshairImage;

    [SerializeField] Color defaultColor = Color.white;

    [SerializeField] Sprite defaultSprite;
    [SerializeField] Vector2 defaultSize = new Vector2(50, 50);

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Awake()
    {
        crosshairImage = GetComponent<Image>();
    }

    private void Start()
    {
        crosshairImage.color = defaultColor;
        crosshairImage.sprite = defaultSprite;

        crosshairImage.rectTransform.sizeDelta = defaultSize;
    }

    private void Update()
    {

        transform.position = Input.mousePosition;

        Vector3 mousePosition = Input.mousePosition;

        float z = mousePosition.z;// + 3;

        //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }
}
