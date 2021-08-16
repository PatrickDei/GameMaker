using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadObject : MonoBehaviour
{

    [SerializeField] UnityEvent anEvent;
    private IList<GameObject> prefabs;

    public void Start()
    {
        Debug.Log("Loading maps");
        Addressables.LoadAssetsAsync<GameObject>("Maps", null).Completed += AddressablesAreReady;
    }

    public void AddressablesAreReady(AsyncOperationHandle<IList<GameObject>> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Addressables ready, invoking!");
            prefabs = handleToCheck.Result;
            anEvent.Invoke();
        }
    }

    public void LoadSingleObject(GameObject target)
    {
        Debug.LogFormat("Amount of addressable prefabs: {0}", prefabs.Count);

        foreach (var obj in prefabs)
        {
            if(obj.name == GameInstance.SharedInstance.MapName)
            {
                GameObject selectedmap = Instantiate(obj, new Vector3(15f, 1f, 30), Quaternion.identity);
                selectedmap.transform.localScale = selectedmap.transform.localScale * 3;
                selectedmap.transform.rotation = Quaternion.AngleAxis(selectedmap.transform.rotation.x - 90, Vector3.right);
                selectedmap.name = obj.name;
                break;
            }
        }
    }
}
