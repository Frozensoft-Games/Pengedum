using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class I18nTextTranslator : MonoBehaviour
{
    public string TextId;

    // Use this for initialization
    void Start()
    {
        var text = GetComponent<Text>();

        if (text != null)
        {
            text.text = TextId == "ISOCode" ? I18n.GetLanguage() : I18n.Fields[TextId];
            return;
        }

        var textTmp = GetComponent<TextMeshProUGUI>();
        if (textTmp == null) return;
        textTmp.text = TextId == "ISOCode" ? I18n.GetLanguage() : I18n.Fields[TextId];
    }
}