using UnityEngine;

public class chr_Traductor : MonoBehaviour
{
    [TextArea(7, 5)]
    [SerializeField] private string _español;
    [TextArea(7, 5)]
    [SerializeField] private string _ingles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (chr_GameManager.Instance.Idioma)
        {
            case Idiomas.Español:
                GetComponent<TMPro.TextMeshProUGUI>().text = _español;
                break;
            case Idiomas.Ingles:
                GetComponent<TMPro.TextMeshProUGUI>().text = _ingles;
                break;
        }
    }
}