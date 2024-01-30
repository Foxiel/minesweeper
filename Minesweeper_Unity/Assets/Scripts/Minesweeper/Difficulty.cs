using System;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Difficulty : MonoBehaviour
{
    public string chosen;
    [SerializeField]
    private NameOption game;
    [SerializeField] 
    private Grid board;
    [SerializeField]
    private Image bg;
    [SerializeField]
    private Image pause;
    private TMP_Text errorText;
    
    private Color _enabledColor = new Color(121f / 255f, 219f / 255f, 201f / 255f, 0f);

    private void Start()
    {
        errorText = GameObject.Find("GeneralError").GetComponent<TMP_Text>();
    }

    public void EasyClick() // This will set the difficulty to easy and start the game
    {
        try
        {
            chosen = "Easy";
            StartGame();
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    public void HardClick() // This will set the difficulty to hard and start the game
    {
        try
        {
            chosen = "Hard";
            StartGame();
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }

    private void StartGame() // This will start the game
    {
        try
        {
            if (game.mode == "single")
            {
                chosen = game.mode + chosen;
                bg.gameObject.SetActive(false);
                gameObject.SetActive(false);
                board.gameObject.SetActive(true);
                Camera.main.backgroundColor = _enabledColor;
                pause.gameObject.SetActive(true);
            }
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }
}
