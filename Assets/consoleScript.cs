using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class consoleScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable ModuleSelectable;
    public TextMesh AllText;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    private KeyCode[] TheKeys =
	{
        KeyCode.Backspace, KeyCode.Return,
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P,
        KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M,
        KeyCode.Space
	};
    string TheLetters = "<Eqwertyuiopasdfghjklzxcvbnm ";
    private bool focused = false;
    private string textOnModule = "";
    int Scenario = -1;
    string input = "";
    string response = "";
    public string[] ordinals = { "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "ninth", "tenth" };
    public string[] numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
    public string[] objectNames = {  };
    public string[] firstObject = {  };
    public string[] secondObject = {  };
    public int[] randomInts = { };
    int relevantObject = -1;
    int theNthObject = -1;
    public string[] objectSelection = {  };
    string correctObj = "";
    int correctOrd = -1;

    void Awake () {
        moduleId = moduleIdCounter++;
        ModuleSelectable.OnFocus += delegate () { focused = true; };
        ModuleSelectable.OnDefocus += delegate () { focused = false; };
    }

    // Use this for initialization
    void Start () {
        Scenario = UnityEngine.Random.Range(0, 1);
        switch (Scenario) {
            case 0: //digital root
            textOnModule = "you see a module with two\nbuttons both with text\nand four displays also\nwith text on them\n\n> $";
            objectNames = new string[] { "BUTTON", "display" }; //all-caps = can interact
            firstObject = new string[] { "yes", "no" };
            randomInts = new int[] { UnityEngine.Random.Range(0,2), UnityEngine.Random.Range(0,10), UnityEngine.Random.Range(0,10), UnityEngine.Random.Range(0,10) };
            correctObj = "button";
            if (randomInts[0] == 0) {
                randomInts[0] = ((((randomInts[1] * 100 + randomInts[2] * 10 + randomInts[3]) - 1) % 9) + 1);
                correctOrd = 0;
            } else {
                randomInts[0] = ((((randomInts[1] * 100 + randomInts[2] * 10 + randomInts[3]) - 1) % 9));
                correctOrd = 1;
            }
            secondObject = new string[] { randomInts[1].ToString(), randomInts[2].ToString(), randomInts[3].ToString(), randomInts[0].ToString() };
            break;
        }
	}

	// Update is called once per frame
	void Update () {
        AllText.text = textOnModule.Replace("$", input);
        for (int i = 0; i < TheKeys.Count(); i++) {
            if (Input.GetKeyDown(TheKeys[i])) {
                if (TheLetters[i].ToString() == "<".ToString()) {
                    handleBack();
                } else if (TheLetters[i].ToString() == "E".ToString()) {
                    handleEnter();
                } else {
                    handleKey(TheLetters[i]);
                }
            }
        }
	}

    void handleKey (char c) {
        input = input + c;
    }

    void handleBack () {
        if (input.Length != 0) {
            input = input.Substring(0, input.Length - 1);
        }
    }

    void handleEnter () {
        relevantObject = -1;
        theNthObject = -1;
        Array.Clear(objectSelection, 0, objectSelection.Count());
        textOnModule = textOnModule.Replace('$'.ToString(), input.ToString());
        textOnModule = textOnModule += "\n\n";
        for (int i = 0; i < objectNames.Count(); i++) { //figures out which object group is relevant
            if (input.Contains(objectNames[i].ToLower()) && relevantObject == -1) {
                relevantObject = i;
            }
        }
        if (relevantObject == -1) {
            response = "the console does not\nunderstand what you are\nasking for";
        } else {
            switch (relevantObject) {
                case 0: objectSelection = (string[])firstObject.Clone(); break;
                case 1: objectSelection = (string[])secondObject.Clone(); break;
                default: Debug.Log("FUCK!"); break;
            }
            for (int n = 0; n < objectSelection.Count(); n++) { //figures out which one it is
                if (input.Contains(ordinals[n]) || input.Contains(numbers[n])) {
                    theNthObject = n;
                }
            }
            if (theNthObject == -1) {
                response = String.Format("i know you are talking\nabout a thing called a\n{0} but which one\nare you talking about", objectNames[relevantObject].ToLower());
            } else {
                //LOOKING AT IT
                if (input.Contains("look") || input.Contains("see") || input.Contains("glance") || input.Contains("glimpse") || input.Contains("peek") || input.Contains("stare") || input.Contains("view") || input.Contains("observe")) {
                    response = String.Format("you look at the {0}\n{1} and it says\n{2} on it", ordinals[theNthObject], objectNames[relevantObject].ToLower(), objectSelection[theNthObject]);
                } else if (input.Contains("press") || input.Contains("push") || input.Contains("tap")) {
                    if (objectNames[relevantObject] == objectNames[relevantObject].ToLower()) {
                        response = String.Format("you go ahead and press\nthe {0} {1}\nbut it does absolutely\nnothing", ordinals[theNthObject], objectNames[relevantObject].ToLower());
                    } else {
                        if (correctObj == objectNames[relevantObject].ToLower() && correctOrd == theNthObject) {
                            response = String.Format("you go ahead and press\nthe {0} {1}\nand you hear the bomb go\nsilent the bomb has been\ndefused", ordinals[theNthObject], objectNames[relevantObject].ToLower());
                            GetComponent<KMBombModule>().HandlePass();
                            //solve animation
                        } else {
                            response = String.Format("you go ahead and press\nthe {0} {1}\nand a buzzing sound is\nplayed you got a strike", ordinals[theNthObject], objectNames[relevantObject].ToLower());
                            GetComponent<KMBombModule>().HandleStrike();
                            //strike animation
                        }
                    }
                } else {
                    response = String.Format("i know you are talking\nabout the {0}\n{1} but what\ndo you want to do with it", ordinals[theNthObject], objectNames[relevantObject].ToLower());
                }
            }
        }
        input = "";
        textOnModule = textOnModule + response + "\n\n> $";
    }
}
