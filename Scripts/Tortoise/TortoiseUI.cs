using UnityEngine;
using TMPro;

public class TortoiseUI : MonoBehaviour
{
    public TMP_Text livesText; 
    public RuntimeScriptable tortoise;

    void Start()
    {
        UpdateLives();
    }

    public void UpdateLives()
    {
        if (livesText != null && tortoise != null)
        {
            livesText.text = "Vidas: " + tortoise.State.Lives;
        }else{
            livesText.text = "No Vidas: " + tortoise.State.Lives;
        }
    }
}
