using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private TMP_Text errorText;

    private void Start()
    {
        errorText = GameObject.Find("GeneralError").GetComponent<TMP_Text>();
    }

    public void ToDesktop() // Quit the game
    {
        try {
            Application.Quit();
        }
        catch (System.Exception e)
        {
            errorText.text = e.Message;
        }
    }

    public void PlayMinesweeper() // Load the Minesweeper scene
    {
        try
        {
            SceneManager.LoadScene("Minesweeper");
        }
        catch (System.Exception e)
        {
            errorText.text = e.Message;
        }
    }
}
