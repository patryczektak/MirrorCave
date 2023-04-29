using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractButton : MonoBehaviour
{

    [SerializeField] string action = "";
    [SerializeField] TMP_Text Hotkey, InteractText;
    [SerializeField] PlayerController assignedPlayer;
    [SerializeField] Color PressedColor, DefaultColor;
    [SerializeField] Image buttonImage;

    bool keyPressed;

    // Start is called before the first frame update
    void Start()
    {
        if (assignedPlayer != null)
        {
            action = assignedPlayer.interactAction;
        }
        Hotkey.text = KeyBindingsManager.GetActionHotkey(action);
        KeyBindingsManager.ContinuousAction(action, () => keyPressed = true);
    }

    // Update is called once per frame
    void Update()
    {
        InteractText.text = "No Action";    
        if (keyPressed)
        {
            buttonImage.color = PressedColor;
            keyPressed = false;
        }
        else
        {
            buttonImage.color = DefaultColor;   
        }
        if (assignedPlayer != null)
        {
            InteractText.text = assignedPlayer.interactName;
        }
    }
}
