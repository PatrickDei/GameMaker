using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using System.Reflection;

public class LoadObject : MonoBehaviour
{

    [SerializeField] UnityEvent anEvent;
    [SerializeField] public string AddressablesGroup;
    private IList<GameObject> prefabs;
    private List<string> names = new List<string>();

    public void OnEnable()
    {
        Debug.LogWarningFormat("Loading {0}", AddressablesGroup);
        Addressables.LoadAssetsAsync<GameObject>(AddressablesGroup, null).Completed += AddressablesAreReady;
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

    public void LoadObjects(GameObject target)
    {
        Debug.LogFormat("Amount of addressable prefabs: {0}", prefabs.Count);

        int i = 0;
        foreach (var obj in prefabs)
        {
            GameObject newObject;

            if (gameObject.scene.name == "LevelSelect")
            {
                newObject = Instantiate(obj, new Vector3(-30f + i++ * 15f, 6.5f, 30), Quaternion.identity);
            }
            else if (obj.name == GameInstance.SharedInstance.MapName)
            {
                newObject = Instantiate(obj, new Vector3(15f, 1f, 30), Quaternion.identity);
                newObject.transform.localScale = newObject.transform.localScale * 3;
            }
            else
                continue;

            Debug.LogFormat("Parent is: {0}", target.name);
            newObject.transform.SetParent(target.transform);
            if(AddressablesGroup != "Figures")
                newObject.transform.rotation = Quaternion.AngleAxis(newObject.transform.rotation.x - 90, Vector3.right);
            newObject.name = obj.name;
        }

        if(AddressablesGroup == "Maps")
            foreach (var obj in Resources.LoadAll<GameObject>("Prefabs/Created Maps"))
            {
                GameObject newObject;

                if (gameObject.scene.name == "LevelSelect")
                {
                    newObject = Instantiate(obj, new Vector3(-30f + i++ * 15f, 6.5f, 30), Quaternion.identity);
                }
                else if (obj.name == GameInstance.SharedInstance.MapName)
                {
                    newObject = Instantiate(obj, new Vector3(15f, -5f, 30), Quaternion.identity);
                    newObject.transform.localScale = newObject.transform.localScale * 3;
                }
                else
                    continue;

                Debug.LogFormat("Parent is: {0}", target.name);
                newObject.transform.SetParent(target.transform);
                if (AddressablesGroup != "Figures")
                    newObject.transform.rotation = Quaternion.AngleAxis(newObject.transform.rotation.x - 90, Vector3.right);
                newObject.name = obj.name;
                newObject.GetComponent<OnClickObject>().AddMapClickListener();
            }
    }

    public void LoadChosenGameObjects(GameObject parent)
    {
        GameObject map = null;
        foreach (var obj in prefabs)
        {
            if (obj.name == GameInstance.SharedInstance.MapName)
            {
                map = Instantiate(obj, parent.transform.position, Quaternion.identity);
                //map.transform.localScale = newObject.transform.localScale * 3;

                Debug.LogFormat("Parent is: {0}", parent.name);
                map.transform.SetParent(parent.transform);
                map.transform.position = parent.transform.position;
                map.name = obj.name;

                if (GameInstance.SharedInstance.MovementStyle == "Free movement")
                {
                    GameInstance.SharedInstance.Fields = new List<KeyValuePair<string, int>>();
                    int i = 0;
                    foreach (Transform child in map.transform.GetChild(0).transform)
                        GameInstance.SharedInstance.Fields.Add(new KeyValuePair<string, int>(child.gameObject.name, i++));
                }

                break;
            }
        }
        if (map == null)
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Created Maps/" + GameInstance.SharedInstance.MapName);
            map = Instantiate(obj, parent.transform.position, Quaternion.identity);
            //map.transform.localScale = newObject.transform.localScale * 3;

            Debug.LogFormat("Parent is: {0}", parent.name);
            map.transform.SetParent(parent.transform);
            map.transform.position = parent.transform.position;
            map.name = obj.name;

            foreach (Transform child in map.transform.GetChild(0).transform)
            {
                child.gameObject.AddComponent<OnClickObject>();
                child.gameObject.GetComponent<OnClickObject>().AddFieldClickListener();
            }

            if (GameInstance.SharedInstance.MovementStyle == "Free movement")
            {
                GameInstance.SharedInstance.Fields = new List<KeyValuePair<string, int>>();
                int i = 0;
                foreach (Transform child in map.transform.GetChild(0).transform)
                    GameInstance.SharedInstance.Fields.Add(new KeyValuePair<string, int>(child.gameObject.name, i++));
            }
        }

        for (int i = 0; i < GameInstance.SharedInstance.Players.Count; i++)
        {
            GameInstance.SharedInstance.Players[i].Figures = new List<KeyValuePair<Figure, int>>();
            for (int j = 0; j < GameInstance.SharedInstance.numOfFiguresPerPlayer; j++) {
                GameObject figure = Instantiate(
                    Resources.Load<GameObject>("prefabs/" + GameInstance.SharedInstance.Players[i].FigureName),
                    map.transform.position,
                    Quaternion.identity);
                figure.transform.position = new Vector3(4f + 1f * j, map.transform.position.y, -6f - 1f * i);
                figure.transform.parent = parent.transform;
                figure.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                string name = UnityEditor.ObjectNames.GetUniqueName(names.ToArray(), figure.name);
                figure.name = name;
                names.Add(figure.name);

                figure.gameObject.GetComponent<OnClickObject>().AddFigureClickListener();

                GameInstance.SharedInstance.Players[i].Figures.Add(new KeyValuePair<Figure, int>(new Figure(figure), -1));
            }
        }

        foreach(Transform field in map.transform.GetChild(0).transform)
            if(GameInstance.SharedInstance.WiningFields.Contains(field.name))
                field.gameObject.GetComponent<Renderer>().material.color = Color.green;

        Destroy(map.GetComponent<BoxCollider>());
        Destroy(map.GetComponent<OnClickObject>());
        Debug.Log("Component deleted on parent!");
    }

    private Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        Debug.LogFormat("Field size: {0}, object {1}", fields.Length, destination.name);
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
        System.Reflection.PropertyInfo[] properties = type.GetProperties(flags);
        Debug.LogFormat("Properties size: {0}, object {1}", properties.Length, destination.name);
        foreach (System.Reflection.PropertyInfo property in properties)
        {
            if(property.CanWrite)
                property.SetValue(copy, property.GetValue(original));
        }

        return copy;
    }
}
