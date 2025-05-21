using UnityEngine;
using UnityEngine.UI;

public class GradientFromFill : MonoBehaviour
{
    [SerializeField] private Gradient gradient;

    private Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();

    }

    private void Update()
    {
        Apply();
    }

    //ContextMenu adds an item to the right-click menu of the compnent
    [ContextMenu("Apply")]
    public void Apply()
    {
        //Make sure we have our references
        if (image == null) { image = GetComponent<Image>(); }

        //Evaluate the gradient based on the image fill amount, and use that color for the image
        image.color = gradient.Evaluate(image.fillAmount);
    }

    //OnValidate runs whenever this component is updated/changed in the inspector
    private void OnValidate()
    {
        Apply();
    }
}
