using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NumberMenu : MonoBehaviour
{
    [SerializeField] NumberButton _numberButtonPrefab;
    [SerializeField] GridLayoutGroup _buttonContainer;

    LevelData[] allLevels;

    private void Start()
    {
        createSectionCards();
    }

    void createSectionCards()
    {
        allLevels = Resources.LoadAll<LevelData>("Levels");
        
        HashSet<int> sectionNameSet = new HashSet<int>();

        for (int i = 0; i < allLevels.Length; i++)
        {
            sectionNameSet.Add(LevelUtils.GetLevelNameInfo(allLevels[i].name)[0]);
        }
        
        foreach (var nameNum in sectionNameSet)
        {
            NumberButton numButton = Instantiate(_numberButtonPrefab, new Vector3(), new Quaternion(), _buttonContainer.transform);
            numButton.SectionNumber = nameNum;
            numButton.ApplyText();
        }
    }
}
