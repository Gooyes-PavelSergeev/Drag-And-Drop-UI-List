using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ElementSortComponent : MonoBehaviour
{
    public GameObject sortButtonsPrefab;

    private GameObject _sortButtons;

    private ElementFiller _filler;

    private static ElementSortComponent _instance;

    private void Start()
    {
        _filler = gameObject.GetComponent<ElementFiller>();
        _instance = this;
    }

    public void AddSortButtons(int numOfElems)
    {
        _sortButtons = _filler.sortButtons;
        _sortButtons.SetActive(true);
        _sortButtons.transform.localPosition = new Vector3(_filler.ElementWidth * numOfElems / 2 + 4, 0, 0);
    }

    public void Reposition()
    {
        _sortButtons.transform.localPosition = new Vector3(_filler.ElementWidth * _filler.NumOfElems / 2 + 4, 0, 0);
    }

    public void SortByString()
    {
        _filler.SortByString();
    }

    public void SortByNum()
    {
        _filler.SortByNum();
    }

    public static void QuickSort(IEnumerable<IComparable> array, int init, int end)
    {
        var newArray = array.ToList();
        if (init < end)
        {
            var pivot = Partition(newArray, init, end);
            QuickSort(newArray, init, pivot - 1);
            QuickSort(newArray, pivot + 1, end);
        }   
    }

    private static int Partition(List<IComparable> array, int init, int end)
    {
        var last = array[end];
        var i = init - 1;
        for (int j = init; j < end; j++)
        {
            if (array[j].CompareTo(last) < 0)
            {
                i++;
                Exchange(array, i, j);     
            }
        }
        Exchange(array, i + 1, end);
        return i + 1;
    }

    private static void Exchange(List<IComparable> array, int i, int j)
    {
        var temp = array[i];
        array[i] = array[j];
        array[j] = temp;

        var tempGameObject = _instance._filler.listElements[i];
        _instance._filler.listElements[i] = _instance._filler.listElements[j];
        _instance._filler.listElements[j] = tempGameObject;
    }

    /*public static void QuickSort(List<int> array, int init, int end)
    {
        if (init < end)
        {
            int pivot = Partition(array, init, end);
            QuickSort(array, init, pivot - 1);
            QuickSort(array, pivot + 1, end);
        }   
    }

    private static int Partition(List<int> array, int init, int end)
    {
        int last = array[end];
        int i = init - 1;
        for (int j = init; j < end; j++)
        {
            if (array[j] <= last)
            {
                i++;
                Exchange(array, i, j);     
            }
        }
        Exchange(array, i + 1, end);
        return i + 1;
    }

    private static void Exchange(List<int> array, int i, int j)
    {
        int temp = array[i];
        array[i] = array[j];
        array[j] = temp;

        var tempGameObject = _instance._filler.listElements[i];
        _instance._filler.listElements[i] = _instance._filler.listElements[j];
        _instance._filler.listElements[j] = tempGameObject;
    }*/
}
