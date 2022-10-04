using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NumberMenu : MonoBehaviour
{
    [SerializeField] NumberButton _numberButtonPrefab;
    [SerializeField] GridLayoutGroup _buttonContainer;

    List<LevelData> allLevels;

    private void Start()
    {
        createSectionCards();
    }

    void createSectionCards()
    {
        allLevels = LevelUtils.GetLevels();
        
        HashSet<int> levelsBySection = new HashSet<int>();

        for (int i = 0; i < allLevels.Count; i++)
        {
            levelsBySection.Add(LevelUtils.GetLevelInfoByName(allLevels[i].name)[0]);
        }
        
        foreach (var nameNum in levelsBySection)
        {
            NumberButton numButton = Instantiate(_numberButtonPrefab, new Vector3(), new Quaternion(), _buttonContainer.transform);
            numButton.SectionNumber = nameNum;
            numButton.ApplyText();
        }
    }
}
