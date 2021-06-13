using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public enum Conditions
{
    NONE,
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
    [Tooltip("The chance, in percentage, that a message box will pop up every time a given state changes. Also note there can only be one text box at a time.")]
    [SerializeField, Range(0,100)] private float messageChance = 10f;
    [SerializeField] private Sound popSound;

    public static FlairTextController instance;

    private Textbox currentTextbox;

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
    }

    public void CreateTextbox(Conditions condition, Vector3 position)
    {
        if(currentTextbox != null)
        {
            print("Will not create textbox; Textbox is already filled");
            return;
        }

        if (Random.Range(0,100) > messageChance)
        {
            print("Will not create textbox; random chance failed");
            return;
        }

        if (condition == Conditions.NONE) return;

        // Get variables
        currentTextbox = Instantiate(textBoxPrefab, position, Quaternion.identity, transform).GetComponent<Textbox>();
        TextAsset conditionAsset = textList.Find(item => item.condition == condition).textAsset;

        // Get text and spawn it
        currentTextbox.Init(GetRandomLineFromTextasset(conditionAsset));

        // Play SFX
        AudioManager.instance.Play(popSound, false);
    }

    private string GetRandomLineFromTextasset(TextAsset ta)
    {
        var arrayString = ta.text.Split('\n');
        return arrayString[Random.Range(0, arrayString.Length)];
    }

}
