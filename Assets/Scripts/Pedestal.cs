using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    private bool _isValidated = false;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _originColor, _validatedColor;
    [SerializeField] private GameObject _activator;
    [SerializeField] private bool _deactivateActivator = false;
    [Header("Optionnal : ")]
    [SerializeField] private PedestalsManager _pedestalsManager;
    [Header("Check the box if you want to\nswitch active state of the objects\nof the others list\nNB : active, the object appears/inactive, the object desappears")]
    [SerializeField] private bool _hasToActivateOthers;
    [SerializeField] private List<GameObject> _others;

    public bool IsValidated { get => _isValidated; }

    // Start is called before the first frame update
    void Start()
    {
        _renderer.material.color = _originColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _activator)
        {
            Activate(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _activator)
        {
            Activate(false);
        }
    }

    private void Activate(bool isValidated)
    {
        _isValidated = isValidated;
        _renderer.material.color = isValidated ? _validatedColor : _originColor;
        _activator.SetActive(!_deactivateActivator);
        if (_hasToActivateOthers && _others.Count > 0)
        {
            foreach (GameObject otherGO in _others)
            {
                otherGO.SetActive(!otherGO.activeSelf);
            }
        }

        if (_pedestalsManager != null)
        {
            _pedestalsManager.UpdatePedestalsManager();
        }
    }
}
