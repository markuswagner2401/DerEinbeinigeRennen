using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ObliqueSenastions.UISpace
{

    public class MyButton : Button
    {
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (!IsActive() || !IsInteractable())
                return;
            UISystemProfilerApi.AddMarker("MyButton:OnPointerExit", this);
            base.OnPointerExit(eventData);
            transition = Selectable.Transition.None;
            // Add the following line to reset the button's state to "Normal"
            //image.color = normalColor;
        }
    }
}
