using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenNLP.Tools.Tokenize;
using Newtonsoft.Json;
using System.IO;

public class TextProcessor : MonoBehaviour {

    public TMP_InputField inputField;

    //path to NLP model
    string modelPath;

    //path to streaming assets
    string streamPath;

	// Use this for initialization
	void Start () {

        //get path to nlp model
        modelPath = Application.dataPath + "/packages/EnglishTok.nbin";

        //get path to streaming assets
        streamPath = Application.streamingAssetsPath + "/TagDatabase.json";

        //get data from json
        string jsonString = File.ReadAllText(streamPath);

        TagDataBase dataBase = JsonConvert.DeserializeObject<TagDataBase>(jsonString);

        print(dataBase.dataBase[0].name);

	}
	
	// Update is called once per frame
	void Update () {
        
        if (inputField.text != "" && Input.GetKey(KeyCode.Return))
        {
            //process the text
            ProcessText(inputField.text);

            inputField.text = "";
        }
	}

    void ProcessText(string input)
    {
        // spell check

        // match words and phrases to tags to create a tag combo

        // match input tags to actions in context

        // execute action

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
        public string[] information;
    }
}
