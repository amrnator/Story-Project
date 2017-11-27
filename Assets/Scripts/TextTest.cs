using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenNLP;
using OpenNLP.Tools.Tokenize;

public class TextTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        var modelPath = Application.dataPath +  "/packages/EnglishTok.nbin";
        var sentence = "- Sorry Mrs. Hudson, I'll skip the tea.";
        var tokenizer = new EnglishMaximumEntropyTokenizer(modelPath);
        var tokens = tokenizer.Tokenize(sentence);

        foreach(string s in tokens)
        {
            print(s);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
