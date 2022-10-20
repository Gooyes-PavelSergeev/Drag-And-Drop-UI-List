using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveData : MonoBehaviour
{
    private ElementFiller _filler;
    private List<GameObject> _elements;
    private List<ElementData> _elementsData;

    private void Start()
    {
        _filler = this.gameObject.GetComponent<ElementFiller>();
    }

    public void SaveIntoJson()
    {
        _elements = _filler.listElements;
        _elementsData = new List<ElementData>();
        foreach (var element in _elements)
        {
            var listElement = element.gameObject.GetComponent<ListElement>();
            var elementData = new ElementData();
            elementData.index = listElement.index;
            elementData.stringText = listElement.stringText.text;
            elementData.numText = listElement.numText.text;
            _elementsData.Add(elementData);
        }
        string listData = _filler.NumOfElems + " ";
        foreach (var elementData in _elementsData){
            listData += JsonUtility.ToJson(elementData);
        }
        System.IO.File.WriteAllText(Application.persistentDataPath + "/ElementsData.json", listData);
    }

    /*public void GetFromJson()
    {
        _elementsData = new List<ElementData>();
        _elementsData = JsonUtility.FromJson<ElementData>()
    }*/
}

[Serializable]
public class ElementData
{
    public int index;
    public string stringText;
    public string numText;
}
