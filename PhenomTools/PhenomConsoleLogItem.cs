using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhenomConsoleLogItem : MonoBehaviour
{
    public static bool altColor;

    [SerializeField]
    private TextMeshProUGUI logText;

    [SerializeField]
    private Image bgImage;

    private int logIndex;

    public void Init(string _text, LogType _logType, int _logIndex)
    {
        gameObject.name = string.Concat("Log_", _logIndex);
        logIndex = _logIndex;

        logText.text = _text;
        logText.color = PhenomConsole.GetColorOfLogType(_logType, 1f, 1f);

        bgImage.color = PhenomConsole.GetColorOfLogType(_logType, altColor ? .1f : .3f, .5f);
        altColor = !altColor;
    }
}
