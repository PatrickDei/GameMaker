using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MapLoader : MonoBehaviour
{
    //public GameObject prefabToLoad;
    IList<GameObject> prefabsToShow;

    void Start()
    {
        Addressables.LoadAssetsAsync<GameObject>("Maps", null).Completed += LoadSpritesWhenReady;
    }

    void LoadSpritesWhenReady(AsyncOperationHandle<IList<GameObject>> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.LogFormat("Number of addressables loaded: {0}", handleToCheck.Result.Count);

            prefabsToShow = handleToCheck.Result;

            int i = 0;
            foreach (var prefab in prefabsToShow)
            {
                GameObject newMap = Instantiate(prefab, new Vector3(-5.2f + i * 15f, 6.5f, 30), Quaternion.identity);
                newMap.name = prefab.name;
                newMap.transform.rotation = Quaternion.AngleAxis(newMap.transform.rotation.x - 90, Vector3.right);
                //prefabToLoad.GetComponent<SpriteRenderer>().sprite = sprite;
                i++;
            }
        }
    }
}
