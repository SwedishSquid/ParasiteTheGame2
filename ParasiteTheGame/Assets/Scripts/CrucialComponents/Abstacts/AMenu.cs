using UnityEngine;

public abstract class AMenu : MonoBehaviour
{
     public void OnPressed() => ButtonSound.Instance.OnPressed();

     public void OnSelected() => ButtonSound.Instance.OnSelected();
     
}