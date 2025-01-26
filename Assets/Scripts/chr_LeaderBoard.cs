using UnityEngine;
using Dan.Main;
using Dan.Models;
using TMPro;

namespace BubbleAbyssLB
{
    public class chr_LeaderBoard : MonoBehaviour
    {

        [SerializeField] private TMP_Text[] _entryTextObjects;
        [SerializeField] private TMP_InputField _UsernameInputField;

        [SerializeField] private chr_GameManager _gameManager;
        private int Score => (int)_gameManager.HighScore;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            LoadEntries();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void LoadEntries()
        {
            // Limpiar todos los textos antes de cargar nuevas entradas
            foreach (var textObject in _entryTextObjects)
                textObject.text = "";

            // Cargar las entradas del leaderboard
            Leaderboards.BubbleAbyssLB.GetEntries(entries =>
            {
                // Asegurarse de no exceder la cantidad de elementos disponibles
                int length = Mathf.Min(_entryTextObjects.Length, entries.Length);

                for (int i = 0; i < length; i++)
                {
                    // Actualizar el texto con la información del leaderboard
                    _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
                }

                // Si hay más textos en _entryTextObjects que entradas, limpiarlos
                for (int i = length; i < _entryTextObjects.Length; i++)
                {
                    _entryTextObjects[i].text = "";
                }
            });
        }

        
        public void UploadEntry()
        {
            Leaderboards.BubbleAbyssLB.UploadNewEntry(_UsernameInputField.text, Score, isSuccessful =>
            {
                if (isSuccessful)
                {
                    Debug.Log("Entry uploaded successfully!");
                    LoadEntries();
                }
                else
                {
                    Debug.LogError("Failed to upload entry.");
                }
            });
        }

    }
}
