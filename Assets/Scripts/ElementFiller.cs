using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

public class ElementFiller : MonoBehaviour
{
    private bool _isInit;
    private bool _hasSortButtons;

    private ElementSortComponent _sortComponent;
    public GameObject sortButtons;
    public GameObject saveButton;
    public GameObject createButton;

    private int _elementWidth = 96;

    public GameObject listElementPrefab;
    public GameObject elementsArea;
    public GameObject listName;

    public List<GameObject> listElements;

    private int _numOfElems;
    private int _maxStringLength = 4;

    public int NumOfElems {get{return _numOfElems;}}
    public int ElementWidth {get {return _elementWidth;}}

    private void Start()
    {
        listElements = new List<GameObject>();
    }

    public void CreateList(int numOfElems)
    {
        if (numOfElems < 1) return;
        _numOfElems = numOfElems;

        if (gameObject.TryGetComponent<ElementSortComponent>(out var sortComponent))
        {
            sortComponent.AddSortButtons(numOfElems);
            _hasSortButtons = true;
            _sortComponent = sortComponent;
        }
        else
        {
            Destroy(sortButtons);
        }

        for (int i = 0; i < numOfElems; i++)
        {
            AddElement(listElementPrefab, i);
        }
        
        listName.SetActive(true);
        saveButton.SetActive(true);
        createButton.SetActive(false);
        SetPosition();
        gameObject.GetComponent<Image>().color = new Vector4 (255f, 255f, 255f, 0.6f);
        UpdateListProperties();
    }

    private void UpdateListProperties()
    {
        listName.GetComponent<TextMeshProUGUI>().text = "Имя: " + this.gameObject.name + ". Кол-во элементов: " + _numOfElems;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (_elementWidth * (_numOfElems + (_hasSortButtons ? 1 : 0)), 200f);
    }

    private void AddElement(GameObject prefab, int index){
        var listElement = Instantiate(prefab, elementsArea.transform);
        listElements.Add(listElement);
        listElement.GetComponent<ListElement>().index = index;
        listElement.GetComponent<ListElement>().stringText.text = GenerateString();
        listElement.GetComponent<ListElement>().numText.text = UnityEngine.Random.Range(0, 100).ToString();
        listElement.GetComponent<ListElement>().Init();
    }

    public void SetPosition()
    {
        for (int i = 0; i < _numOfElems; i++)
        {
            listElements[i].GetComponent<ListElement>().index = i;
            listElements[i].transform.localPosition = new Vector3 (_elementWidth * (1 - _numOfElems - (_hasSortButtons ? 1 : 0) + 2 * i) / 2, 0, 0);
        }
    }

    private string GenerateString()
    {
        int stringlen = UnityEngine.Random.Range(2, _maxStringLength);
        int randValue;
        string str = "";
        char letter;
        for (int i = 0; i < stringlen; i++)
        {
            randValue = UnityEngine.Random.Range(0, 26);
            letter = Convert.ToChar(randValue + 65);
            str = str + letter;
        }
        return str;
    }

    public void RemoveElement (ListElement draggableElement)
    {
        listElements.RemoveAt(draggableElement.index);
        draggableElement.Recycle();
        _numOfElems--;
        SetPosition();
        UpdateListProperties();
        if (_hasSortButtons) _sortComponent.Reposition();
    }

    public void AddElement(int index, ListElement element)
    {
        var listElement = Instantiate(element.gameObject, elementsArea.transform);
        listElements.Insert(index, listElement);
        _numOfElems++;
        SetPosition();
        UpdateListProperties();
        if (_hasSortButtons) _sortComponent.Reposition();
    }

    public void SortByString()
    {
        var stringList = new List<string>();
        for (int i = 0; i < listElements.Count; i++)
        {
            stringList.Add(listElements[i].GetComponent<ListElement>().elementString);
        }
        IEnumerable<IComparable> comparables = stringList;
        ElementSortComponent.QuickSort(comparables, 0, stringList.Count - 1);
        SetPosition();
    }

    public void SortByNum()
    {
        var numList = new List<int>();
        for (int i = 0; i < listElements.Count; i++)
        {
            numList.Add(listElements[i].GetComponent<ListElement>().elementNum);
        }
        var comparables = numList.Cast<IComparable>();
        ElementSortComponent.QuickSort(comparables, 0, numList.Count - 1);
        SetPosition();
    }
}
