using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject: MonoBehaviour, IPointerClickHandler
{
    // this method will calles when we click on a object
    public void OnPointerClick(PointerEventData eventData)
    {

        // get the IVizualizer from an object
        IVizualizer vizualizer = GetComponent<IVizualizer>();

        if (vizualizer != null)
        {
            // call the functionality of the object, if it have component vizualizer
            vizualizer.ShowData();
        }
    }
}
