using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Vector4Int
{
    public int x;
    public int y;
    public int z;
    public int w;

    public override string ToString()
    {
        return x.ToString() + y.ToString() + z.ToString() + w.ToString();
    }
}
public class UIDigicode : MonoBehaviour
{
    [SerializeField] private Vector4Int _password;
    [SerializeField] private TMP_Text _outputText;
    [SerializeField] private GameObject _inputButtonPrefab;
    [SerializeField] private GridLayoutGroup _inputButtonsGroup;
    private List<GameObject> _inputButtons = new List<GameObject>();
    private bool _isValid = false;
    public bool IsValid { get { return _isValid; } }
    public GameObject popup;
    // Start is called before the first frame update
    void Start()
    {
        if (_outputText == null || _inputButtonPrefab == null || _inputButtonsGroup == null) return;
        for (int i = 0; i < 12; i++)
        {
            GameObject button = Instantiate(_inputButtonPrefab, _inputButtonsGroup.transform);
            _inputButtons.Add(button);
            if (i == 0)
            {
                button.GetComponentInChildren<TMP_Text>().text = "Suppr";
                button.GetComponent<Button>().onClick.AddListener(DeleteOutput);
            }
            else if (i == 2)
            {
                button.GetComponentInChildren<TMP_Text>().text = "Reset";
                button.GetComponent<Button>().onClick.AddListener(EnterOutput);
            }
            else if (i == 1)
            {
                button.GetComponentInChildren<TMP_Text>().text = "0";
                button.GetComponent<Button>().onClick.AddListener(() => PressDigit('0'));
            }
            else
            {
                int digit = i - 2;
                button.GetComponentInChildren<TMP_Text>().text = digit.ToString();
                button.GetComponent<Button>().onClick.AddListener(() => PressDigit(digit.ToString()[0]));
            }
        }
    }

    private void PressDigit(char digit)
    {
        if (_outputText == null) return;
        _outputText.text = _outputText.text.Remove(0, 1) + digit;
        print(digit);
        if (_outputText.text.Contains('_')) return;
        if (_outputText.text == _password.ToString())
        {
            foreach (GameObject button in _inputButtons)
            {
                button.GetComponent<Button>().enabled = false;
            }
            _outputText.GetComponentInParent<Image>().color = Color.green;
            _isValid = true;
            popup.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            _outputText.text = "____";
        }
    }

    private void EnterOutput()
    {
        if (_outputText == null) return;
        _outputText.text = "____";
    }

    private void DeleteOutput()
    {
        if (_outputText == null) return;
        if (_outputText.text.Length > 0)
            _outputText.text = "_" + _outputText.text.Remove(_outputText.text.Length - 1);
    }
}
//            if (i > 2)
//            {
//                GameObject inputButton = Instantiate(_inputButtonPrefab, _inputButtonsGroup.transform);
//                inputButton.GetComponentInChildren<TMP_Text>().text =  (i-2).ToString();
//                inputButton.GetComponent<Button>().onClick.AddListener(PressDigit);
//            }
//            else
//            {

//            }
//        }
//    }
//    public void PressDigit()
//    {
//    }
//    public void DeleteOutput()
//    {

//    }
//    public void EnterOutput()
//    {

//    }
//}
