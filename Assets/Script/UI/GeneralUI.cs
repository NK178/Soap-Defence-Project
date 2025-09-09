using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{

    [SerializeField] protected Image image; 


    public Image GetImage()
    {
        if (image != null)
            return image;
        else 
            return null;    
    }


    public void SetImage(Image image)
    {
        this.image = image;
    }

}
