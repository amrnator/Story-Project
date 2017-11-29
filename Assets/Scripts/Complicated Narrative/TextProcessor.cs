using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenNLP.Tools.Tokenize;
using Newtonsoft.Json;
using System.IO;
using System;

public class TextProcessor : MonoBehaviour {

    public TMP_InputField inputField;

    //path to NLP model
    string modelPath;

    //path to streaming assets
    string streamPath;

    TagDataBase tagDataBase;

	// Use this for initialization
	void Start () {

        //get path to nlp model
        modelPath = Application.dataPath + "/packages/EnglishTok.nbin";

        //get path to streaming assets
        streamPath = Application.streamingAssetsPath + "/TagDatabase.json";

        //get data from json
        String jsonString = File.ReadAllText(streamPath);

        tagDataBase = JsonConvert.DeserializeObject<TagDataBase>(jsonString);

        print(tagDataBase.dataBase[0].name);
    }
	
	void Update () {

        //process text when submitted from the input field
        if (inputField.text != "" && Input.GetKey(KeyCode.Return))
        {
            //process the text
            ProcessText(inputField.text);

            inputField.text = "";
        }
	}

    private void ProcessText(string input)
    {
        // spell check

        // match words and phrases to tags to create a tag combo
        List<string> inputTags = ExtractTags(input);

        // match input tags to actions in context

        // execute action

    }

    private List<string> ExtractTags(String input){

        List<String> tags = new List<String>();

        //tokenize string
        var tokenizer = new EnglishRuleBasedTokenizer(true);

        var tokens = tokenizer.Tokenize(input);

        //print tokens
        foreach (String x in tokens){
            print(x);
        }

        //TODO implement this
        foreach (String x in tokens)
        {
            foreach (Tag dataTag in tagDataBase.dataBase)
            {
                foreach (String y in dataTag.information)
                {
                    if (x.CaseInsensitiveContains(y))
                    {
                        // add tag to list
                        tags.Add(dataTag.name);
                    }
                }
            }
        }

        print("TAGS: ");

        //print tokens
        foreach (String x in tags)
        {
            print(x);
        }

        return tags;
    }

    //takes the input tags and selects an appropraite action from the context
    private void FindAction(List<String> inputTags){

        return;
    }

    [System.Serializable]
    public class TagDataBase
    {
        public Tag[] dataBase;
    }

    [System.Serializable]
    public class Tag
    {
        public int tagID;
        public string name;
        public String[] information;
    }

}

public static class Extensions
{
    public static bool CaseInsensitiveContains(this string text, string value,
        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
    {
        return text.IndexOf(value, stringComparison) >= 0;
    }
}
