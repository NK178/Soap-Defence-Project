using UnityEngine;
using TMPro;



public class GeneralTextUI : MonoBehaviour
{

    [SerializeField] protected TMP_Text display;
  
    public TMP_Text GetText()
    {
        if (display != null)
            return display;
        else 
            return null;
    }

    public void SetText(TMP_Text text)
    {
        display = text;
    }
}
