using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class KeyBinding
{
    public string action;
    public KeyCode keyCode;
}

public class KeyBindingsManager : MonoBehaviour
{
    public static KeyBindingsManager Instance { get; private set; }
    public List<KeyBinding> keyBindings;

    private Dictionary<string, List<UnityAction>> actionEvents;
    private Dictionary<string, List<UnityAction>> continuousActionEvents;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            actionEvents = new Dictionary<string, List<UnityAction>>();
            continuousActionEvents = new Dictionary<string, List<UnityAction>>();
            foreach (KeyBinding binding in keyBindings)
            {
                if (!actionEvents.ContainsKey(binding.action))
                {
                    actionEvents[binding.action] = new List<UnityAction>();
                    continuousActionEvents[binding.action] = new List<UnityAction>();
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        foreach (KeyBinding binding in keyBindings)
        {
            if (Input.GetKeyDown(binding.keyCode))
            {
                if (actionEvents.ContainsKey(binding.action))
                {
                    foreach (UnityAction action in actionEvents[binding.action])
                    {
                        action.Invoke();
                    }
                }
            }

            if (Input.GetKey(binding.keyCode))
            {
                if (continuousActionEvents.ContainsKey(binding.action))
                {
                    foreach (UnityAction action in continuousActionEvents[binding.action])
                    {
                        action.Invoke();
                    }
                }
            }
        }
    }

    public static void Action(string action, UnityAction actionCallback)
    {
        if (Instance != null && Instance.actionEvents.ContainsKey(action))
        {
            Instance.actionEvents[action].Add(actionCallback);
        }
    }

    public static void ContinuousAction(string action, UnityAction actionCallback)
    {
        if (Instance != null && Instance.continuousActionEvents.ContainsKey(action))
        {
            Instance.continuousActionEvents[action].Add(actionCallback);
        }
    }
}
