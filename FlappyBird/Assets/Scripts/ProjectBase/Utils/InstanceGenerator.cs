using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _generateCount;

    private void Awake()
    {
        int count = _generateCount;
        while (count > 0)
        {
            count--;
            GameObject obj = GameObject.Instantiate(_prefab);
            obj.transform.parent = transform;
        }
 
    }

}
