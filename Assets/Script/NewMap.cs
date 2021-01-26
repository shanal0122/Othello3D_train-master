using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NewMap : MonoBehaviour
{
    public Texts texts;
    int columnLength;
    string textstr;

    void Start()
    {
      textstr = texts.readText("/Resources/TextFileWrite.txt");
      string[] textMessage = textstr.Split('\n');
      string[] tempWords = textMessage[0].Split(',');
      columnLength = tempWords.Length;

      if(columnLength == 355)
      {
        for (int n = 0; n < columnLength-1; n++)
        {
            if(n==0){if(texts.saveText("/Resources/TextFileShapeFixed.txt", "{ {")){}else{Debug.Log("Error;");}}
            if(texts.saveText("/Resources/TextFileShapeFixed.txt", tempWords[n].ToString() + "f")){}else{Debug.Log("Error;");}
            if(n <= 191)
            {
              if((n+1) % 64 == 0){if(texts.saveText("/Resources/TextFileShapeFixed.txt", "} },\n\n\n{ {")){}else{Debug.Log("Error;");}}
              else if((n+1) % 16 == 0){if(texts.saveText("/Resources/TextFileShapeFixed.txt", "} },\n{ {")){}else{Debug.Log("Error;");}}
              else if((n+1) % 4 == 0){if(texts.saveText("/Resources/TextFileShapeFixed.txt", "}, {")){}else{Debug.Log("Error;");}}
              else{if(texts.saveText("/Resources/TextFileShapeFixed.txt", ", ")){}else{Debug.Log("Error;");}}
            }
            if(n==191){if(texts.saveText("/Resources/TextFileShapeFixed.txt", "\n\n\n{\n{ {")){}else{Debug.Log("Error;");}}
            if(n >= 192)
            {
              if((n-191) % 81 == 0){if(texts.saveText("/Resources/TextFileShapeFixed.txt", "} }\n},\n\n\n{\n{ {")){}else{Debug.Log("Error;");}}
              else if((n-191) % 27 == 0){if(texts.saveText("/Resources/TextFileShapeFixed.txt", "} }\n},\n{\n{ {")){}else{Debug.Log("Error;");}}
              else if((n-191) % 9 == 0){if(texts.saveText("/Resources/TextFileShapeFixed.txt", "} },\n{ {")){}else{Debug.Log("Error;");}}
              else if((n-191) % 3 == 0){if(texts.saveText("/Resources/TextFileShapeFixed.txt", "}, {")){}else{Debug.Log("Error;");}}
              else{if(texts.saveText("/Resources/TextFileShapeFixed.txt", ", ")){}else{Debug.Log("Error;");}}
            }
        }
      }
      EditorApplication.isPlaying = false;
    }
}
