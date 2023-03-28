using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private bool _isLightSwitch = false, _isActivator = false, _isDoorActivator = false, _isTeleporter = false;
    [SerializeField] private Door _door;
    [SerializeField] private List<GameObject> _others;
    [SerializeField] private List<Light> _lights;
    [SerializeField] private Transform _teleportationTarget;
    [SerializeField] private List<float> _initialLightIntensities;
    [SerializeField] private float _defaultInitialLightIntersity = 10;
    private Key _myKey = null;
    private bool _canTeleport = true;
    private void Update()
    {
        if (_door != null) _canTeleport = _door.CanTeleport;
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if (other.tag != "Player" && other.tag != "Key") return;
        if (other.tag == "Key")
        {
            print("key detected");
            _myKey = other.GetComponent<Key>();
            if (_isDoorActivator && _door != null && _door.NeedKey && _myKey != null)
            {
                _door.ActivateDoor(_myKey);
            }
        }
        else
        {
            print("player detected");
            if(_isActivator) ActivateAllOthers();
            if (_isLightSwitch) SwitchLights();
            if (_isDoorActivator && _door != null && !_door.NeedKey)
            {
                print("call activate door");
                _door.ActivateDoor();
            }


        }
    }
    private void ActivateAllOthers(){
        foreach(GameObject go in _others){
            go.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;
        if (_isTeleporter &&
            _teleportationTarget != null &&
            other.transform != null &&
            (!_isDoorActivator || (_isDoorActivator && (!_door.NeedKey || (_door.NeedKey && _myKey != null && _myKey.IsGoodKey(_door))))))
        {
            StartCoroutine(Teleport(other.transform, _teleportationTarget));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Key") return;
        if (_myKey == other.GetComponent<Key>()) _myKey = null;
    }

    private IEnumerator Teleport(Transform playerTransform = null, Transform targetTransform = null)
    {
        while (!_canTeleport)
        {
            yield return null;
        }
        playerTransform.position = targetTransform.position;
        playerTransform.rotation = targetTransform.rotation;
    }

    private void SwitchLights()
    {
        if (_lights.Count == 0) return;
        else
        {
            for (int i = 0; i < _lights.Count; i++)
            {

                if (i < _initialLightIntensities.Count)
                {
                    if (_initialLightIntensities[i] == -1) _initialLightIntensities[i] = _lights[i].intensity;
                }
                else _initialLightIntensities.Add(_lights[i].intensity == 0 ? _defaultInitialLightIntersity : _lights[i].intensity);
                _lights[i].intensity = _lights[i].intensity == 0 ? _initialLightIntensities[i] : 0;
            }
        }
    }
}
