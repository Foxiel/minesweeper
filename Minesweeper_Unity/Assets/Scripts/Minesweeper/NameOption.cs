using TMPro;
using UnityEngine;

public class NameOption : MonoBehaviour
{
    public TMP_InputField userName;
    [SerializeField]
    private TMP_Text error;
    [SerializeField]
    private GameObject difficulty;
    [SerializeField]
    private GameObject hostorjoin;
    private TMP_Text errorText;
    
    public string mode;
    
    private Color disabledColor = new Color(111f / 255f, 152f / 255f, 144f / 255f, 0f);
    
    void Start()
    {
        try
        {
            Camera.main.backgroundColor = disabledColor;
        }
        catch (System.Exception e)
        {
            errorText.text = e.Message;
        }
    }

    public void singlePlayerClick() // This will run if singleplayer has been clicked
    {
        try
        {
            if (!string.IsNullOrEmpty(userName.text))
            {
                mode = "single";
                GameObject.Find("SingleMultiplayer").SetActive(false);
                difficulty.SetActive(true);
            }
            else
            {
                error.gameObject.SetActive(true);
            }
        }
        catch (System.Exception e)
        {
            errorText.text = e.Message;
        }
    }
}
