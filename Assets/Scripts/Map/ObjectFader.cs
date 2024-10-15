using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    [Header("Fade settings")]
    [Tooltip("Speed at which object fades")] public float fadeSpeed = 10f;
    public float fadeAmount = 0.5f;

    float orginalOpacity; //Original opacity of the object

    Material material;

    public bool DoFade = false;

    private void Awake()
    {
        material = GetComponent<Renderer>().material; //Get the material of the object
    }

    private void Start()
    {    
        orginalOpacity = material.color.a;   //Get the original opacity of the object
    }

    private void Update()
    {
        if (DoFade)
        {
            Fade(); //Fade the object
        }
        else
        {
            ResetFade(); //Reset the fade of the object
        }
    }

    private void Fade()
    {
        Color curreColor = material.color;
        Color smootColor = new Color(curreColor.r, curreColor.g, curreColor.b, Mathf.Lerp(curreColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
        material.color = smootColor;
    }

    public void ResetFade()
    {
        Color curreColor = material.color;
        Color smootColor = new Color(curreColor.r, curreColor.g, curreColor.b, Mathf.Lerp(curreColor.a, orginalOpacity, fadeSpeed * Time.deltaTime));
        material.color = smootColor;
    }
}
