using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
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

        List<int> sortedLevels = levelsBySection.ToList();

        while(true){
            bool isChanged = false;

            for (int i = 0; i < sortedLevels.Count - 1; i++)
            {
                int current = sortedLevels[i];
                int next = sortedLevels[i + 1];

                if(current > next){
                    int temp = next;
                    sortedLevels[i + 1] = current;
                    sortedLevels[i] = temp;
                    isChanged = true;
                }
            }

            if(isChanged){
                isChanged = false;
            } else
            {
                break;
            }
        }
        
        foreach (var nameNum in sortedLevels)
        {
            NumberButton numButton = Instantiate(_numberButtonPrefab, new Vector3(), new Quaternion(), _buttonContainer.transform);
            numButton.SectionNumber = nameNum;
            numButton.ApplyText();
        }
    }
}
