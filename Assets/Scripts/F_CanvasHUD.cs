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
            if (chr_GameManager.Instance.GameState == GameState.PlayGame || chr_GameManager.Instance.GameState == GameState.ResumeGame)
            {
                hudCanvas.SetActive(true);
            }
            else if (chr_GameManager.Instance.GameState == GameState.Pause)
            {
                hudCanvas.SetActive(false);
            }
        }
    }
}
