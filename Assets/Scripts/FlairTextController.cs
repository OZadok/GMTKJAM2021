using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;

public enum Conditions
{
    FIRE_BUILDING,
    FIRE_PEEP,
    FIRE_CRIMEDEN,
    FIRE_CAR,
    BUILDING_CRIMEDEN,
    PEEP_WOUNDED,
}

[System.Serializable]
public class TextConditionals
{
    public Conditions condition;
    //public string fileName;
    public TextAsset textAsset;
}

public class FlairTextController : MonoBehaviour
{
    [SerializeField] private List<TextConditionals> textList = new List<TextConditionals>();
    [SerializeField] private GameObject textBoxPrefab;

    public static FlairTextController instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start()
    {
        //var path = Application.dataPath + "/Writing/" + textList[0].fileName + ".txt";
        var path = AssetDatabase.GetAssetPath(textList[0].textAsset);
        string[] lines = File.ReadAllLines(path);
        //print(lines[Random.Range(0, lines.Length)]);
    }
}
