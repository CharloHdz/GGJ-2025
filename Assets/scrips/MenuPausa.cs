using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa: MonoBehaviour


{
[SerializeField] private GameObject botonPausa;
[SerializeField] private GameObject menuPausa;


public void Pausa(){
  Time.timeScale = 0;
  botonPausa.SetActive(false);
  menuPausa.SetActive(true);

}

public void Reanudar(){

    Time.timeScale = 1f;
    botonPausa.SetActive(true);
    menuPausa.SetActive(false);
}
public void Inicio(){

    Time.timeScale = 1;
     SceneManager.LoadScene("MenuPrincipal");
}


    
}
