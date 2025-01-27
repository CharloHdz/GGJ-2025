using UnityEngine;

public class F_CanvasHUD : MonoBehaviour
{
    [SerializeField] private GameObject hudCanvas;
    private void Awake()
    {
        hudCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            hudCanvas.SetActive(!hudCanvas.activeSelf);
        }
    }
}
