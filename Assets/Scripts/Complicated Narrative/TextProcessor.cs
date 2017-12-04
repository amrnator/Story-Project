using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenNLP.Tools.Tokenize;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;

public class TextProcessor : MonoBehaviour {

    public TMP_InputField inputField;

    PlayerController playerController;

    //path to NLP model
    string modelPath;

    //path to streaming assets
    string streamPath;

    //database of tags and associated words
    TagDataBase tagDataBase;

    //database of tag combos and associated responses
    ResponseDataBase responseDatabase;

	// Use this for initialization
	void Start () {

        //get playerController
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        //get path to nlp model
        modelPath = Application.dataPath + "/packages/EnglishTok.nbin";

        //get path to streaming assets
        streamPath = Application.streamingAssetsPath;

        //get tagDatabase data from json
        String jsonString = File.ReadAllText(streamPath + "/TagDatabase.json");

        tagDataBase = JsonConvert.DeserializeObject<TagDataBase>(jsonString);

        //get ResponseDatabase
        jsonString = File.ReadAllText(streamPath + "/ResponseDatabase.json");

        responseDatabase = JsonConvert.DeserializeObject<ResponseDataBase>(jsonString);

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
        List<String> inputTags = ExtractTags(input);

        // match input tags to actions in context
        Response r = FindAction(inputTags);

        print(r.response);

        // execute action
        playerController.performResponse(r);

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

    //takes the input tags and selects an appropraite action from the ResponseDatase
    private Response FindAction(List<String> inputTags){

        foreach(Response r in responseDatabase.dataBase){
            if (inputTags.SequenceEqual(r.tagCombo)){
                return r;
            }
        }

        return null;
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

    [System.Serializable]
    public class ResponseDataBase
    {
        public Response[] dataBase;
    }

    [System.Serializable]
    public class Response
    {
        public String[] tagCombo;
        public string response;
        public string function;
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
