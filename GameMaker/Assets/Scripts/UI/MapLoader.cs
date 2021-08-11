using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MapLoader : MonoBehaviour
{
    public GameObject prefabToLoad;

    void Start()
    {
        Addressables.LoadAssetsAsync<Sprite>("Maps", null).Completed += LoadSpritesWhenReady;
    }

    void LoadSpritesWhenReady(AsyncOperationHandle<IList<Sprite>> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log(handleToCheck.Result.Count);

            int i = 0;
            foreach (var sprite in handleToCheck.Result)
            {
                if (prefabToLoad != null)
                {
                    GameObject newMap = Instantiate(prefabToLoad, new Vector3(-5.2f + i * 15f, 6.5f, 30), Quaternion.identity);
                    newMap.name = sprite.name;
                    prefabToLoad.GetComponent<SpriteRenderer>().sprite = sprite;
                    i++;
                }
            }
        }
    }
}
