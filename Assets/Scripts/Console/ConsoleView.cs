using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleView : MonoBehaviour {

    ConsoleController console = new ConsoleController();

    public GameObject viewContainer; 
    public Text logTextArea;
    public InputField inputField;

    void Start () {
        if (console != null)
        {
            console.visibilityChanged += onVisibilityChanged;
            console.logChanged += onLogChanged;
        }
        updateLogStr(console.log);
    }
    
    ~ConsoleView()
    {
        console.visibilityChanged -= onVisibilityChanged;
        console.logChanged -= onLogChanged;
    }

    void Update () {
        //visibility
        if (Input.GetKeyDown("`") && !inputField.isFocused)
            toggleVisibility();
    }

    public void Log(string message)
    {
        console.appendLogLine(message);
    }

    private void toggleVisibility()
    {
        setVisibility(!viewContainer.activeSelf);
    }

    private void setVisibility(bool visible)
    {
        viewContainer.SetActive(visible);
    }

    private void onVisibilityChanged(bool visible)
    {
        setVisibility(visible);
    }

    private void onLogChanged(string[] newLog)
    {
        updateLogStr(newLog);
    }

    private void updateLogStr(string[] newLog)
    {
        if (newLog == null)
            logTextArea.text = "";
        else
            logTextArea.text = string.Join("\n", newLog);
    }

    
	public void runCommand()
    {
        console.runCommandString(inputField.text);
        inputField.text = "";
    }
}
