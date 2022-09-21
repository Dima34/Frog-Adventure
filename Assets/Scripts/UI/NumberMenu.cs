using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberMenu : MonoBehaviour
{
    [SerializeField] int _numberSectionsAmount = 10;
    [SerializeField] NumberButton _numberButtonPrefab;
    [SerializeField] GridLayoutGroup _buttonContainer;

    private void Start() {
        for (int i = 0; i < _numberSectionsAmount; i++)
        {
            Debug.Log("i - " + i);
            NumberButton numButton = Instantiate(_numberButtonPrefab, new Vector3(), new Quaternion(), _buttonContainer.transform);
            numButton.SectionNumber = i + 1;
            numButton.applyText();
        }
    }
}
