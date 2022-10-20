using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ListElement : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index;

    public TextMeshProUGUI stringText;
    public TextMeshProUGUI numText;

    public string elementString;
    public int elementNum;

    private ElementFiller _filler;

    public event Action<PointerEventData> onBeginDragHandler;
	public event Action<PointerEventData> onDragHandler;
	public event Action<PointerEventData, bool> onEndDragHandler;

	public bool FollowCursor { get; set; } = true;

	public Vector3 startPosition;

	public bool CanDrag { get; set; } = true;

    private RectTransform rectTransform;
	private Canvas canvas;

    public void Init(){
        elementString = stringText.text;
        elementNum = Int32.Parse(numText.text);
    }

    private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>();
        _filler = GetComponentInParent<ElementFiller>();
        this.gameObject.GetComponent<Image>().enabled = true;
        this.enabled = true;
        stringText.enabled = true;
        numText.enabled = true;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!CanDrag)
		{
			return;
		}

		onBeginDragHandler?.Invoke(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!CanDrag)
		{
			return;
		}

		onDragHandler?.Invoke(eventData);

		if (FollowCursor)
		{
			rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!CanDrag)
		{
			return;
		}

		var results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, results);

		DropArea dropArea = null;
        ListElement elementToReplace = null;

		foreach (var result in results)
		{
			dropArea = result.gameObject.GetComponent<DropArea>();
            if (result.gameObject.TryGetComponent<ListElement>(out var listElement))
            {
                if (listElement.gameObject != this.gameObject)
                {
                    elementToReplace = listElement;
                }
            }

			if (dropArea != null && elementToReplace != null)
			{
				break;
			}
		}

		if (dropArea != null && elementToReplace != null && _filler.NumOfElems > 1)
		{
			dropArea.Drop(this, elementToReplace);
			onEndDragHandler?.Invoke(eventData, true);
			return;
		}

		rectTransform.anchoredPosition = startPosition;
		onEndDragHandler?.Invoke(eventData, false);
	}

	public void OnInitializePotentialDrag(PointerEventData eventData)
	{
		startPosition = rectTransform.anchoredPosition;
	}

    public void Recycle()
    {
        Destroy(this.gameObject);
    }
}
