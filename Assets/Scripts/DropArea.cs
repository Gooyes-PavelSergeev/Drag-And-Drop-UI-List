using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DropArea : MonoBehaviour
{
	public event Action<ListElement> onDropHandler;

    private ElementFiller _filler;

    private void Start()
    {
        _filler = GetComponent<ElementFiller>();
    }

	public void Drop(ListElement draggable, ListElement elementToReplace)
	{
        var fillerFrom = draggable.gameObject.GetComponentInParent<ElementFiller>();
        var fillerTo = elementToReplace.gameObject.GetComponentInParent<ElementFiller>();
        var indexFrom = draggable.index;
        var indexTo = elementToReplace.index;
        fillerFrom.RemoveElement(draggable);
        fillerTo.AddElement(indexTo, draggable);
		onDropHandler?.Invoke(draggable);
	}
}
