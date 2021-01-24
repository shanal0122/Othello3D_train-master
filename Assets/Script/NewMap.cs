using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NewMap : MonoBehaviour
{
    public Texts texts;
    private float[,] mapsq;
    private float[] mapsqAve;
    int columnLength;
    int rowLength;

    string[] textstr;
    string[] newtext;

    void Start()
    {
      textstr = new string[6];
      textstr[1] = texts.readText("/Resources/TextFile1Deta.txt");
      textstr[2] = texts.readText("/Resources/TextFile2Deta.txt");
      textstr[3] = texts.readText("/Resources/TextFile3Deta.txt");
      textstr[4] = texts.readText("/Resources/TextFile4Deta.txt");
      textstr[5] = texts.readText("/Resources/TextFile5Deta.txt");
      string[] textMessage1 = textstr[1].Split('\n');
      string[] textMessage2 = textstr[2].Split('\n');
      string[] textMessage3 = textstr[3].Split('\n');
      string[] textMessage4 = textstr[4].Split('\n');
      string[] textMessage5 = textstr[5].Split('\n');
      rowLength = textMessage1.Length;

      newtext = new string[rowLength-1];
      for(int n=0; n<rowLength-1; n++)
      {
        newtext[n] = textMessage1[n] + textMessage2[n] + textMessage3[n] + textMessage4[n] + textMessage5[n];
      }
      Mokuji();
      for(int n=0; n<rowLength-1; n++)
      {
        if(texts.saveText("/Resources/TextFileAve.txt", newtext[n] + "\n")){}else{Debug.Log("Error;");}
      }
      Debug.Log("New Map is created. Please check TextFileAve.txt");
      string[] tempWords = newtext[1].Split(',');
      //Debug.Log(tempWords.Length);
      EditorApplication.isPlaying = false;
    }

    private void Mokuji()
    {
      for(int y=0; y<4; y++)
      {
        for(int z=0; z<4; z++)
        {
          for(int x=0; x<4; x++)
          {
            if(texts.saveText("/Resources/TextFileAve.txt", "ba1" + y + z + x + ",")){}else{Debug.Log("Error;");}
          }
        }
      }

      for(int y=0; y<4; y++)
      {
        for(int z=0; z<4; z++)
        {
          for(int x=0; x<4; x++)
          {
            if(texts.saveText("/Resources/TextFileAve.txt", "ba2" + y + z + x + ",")){}else{Debug.Log("Error;");}
          }
        }
      }

      for(int y=0; y<4; y++)
      {
        for(int z=0; z<4; z++)
        {
          for(int x=0; x<4; x++)
          {
            if(texts.saveText("/Resources/TextFileAve.txt", "ba3" + y + z + x + ",")){}else{Debug.Log("Error;");}
          }
        }
      }

      for(int a1=0; a1<3; a1++)
      {
        for(int a2=0; a2<3; a2++)
        {
          for(int a3=0; a3<3; a3++)
          {
            for(int a4=0; a4<3; a4++)
            {
              if(texts.saveText("/Resources/TextFileAve.txt", "si0" + a1 + a2 + a3 + a4 + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
      }

      for(int a1=0; a1<3; a1++)
      {
        for(int a2=0; a2<3; a2++)
        {
          for(int a3=0; a3<3; a3++)
          {
            for(int a4=0; a4<3; a4++)
            {
              if(texts.saveText("/Resources/TextFileAve.txt", "su0" + a1 + a2 + a3 + a4 + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
      }
      if(texts.saveText("/Resources/TextFileAve.txt", "\n")){}else{Debug.Log("Error;");}
    }




    /*void Start()
    {
      for(int t=1; t<6; t++)
      {
        GetList(t);
        Cul();
        SaveTextAve(t);
      }
      Debug.Log("New Map is created. Please check TextFileAve.txt");
      EditorApplication.isPlaying = false;
    }

    private void GetList(int t)
    {
        string textstr = texts.readText("/Resources/TextFile" + t + "Deta.txt");
        string[] textMessage = textstr.Split('\n'); //

        //行数と列数を取得
        columnLength = textMessage[0].Split(',').Length;
        rowLength = textMessage.Length;

        //2次配列を定義
        mapsq = new float[rowLength-1, columnLength-1];
        mapsqAve = new float[columnLength-1];

        for(int i = 0; i < rowLength-1; i++)
        {

            string[] tempWords = textMessage[i].Split(','); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < columnLength-1; n++)
            {
                mapsq[i, n] = float.Parse(tempWords[n]); //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }
    }

    private void Cul()
    {
      for (int n = 0; n < columnLength-1; n++)
      {
          mapsqAve[n] = 0;
          for(int i = 0; i < rowLength-1; i++)
          {
              mapsqAve[n] += mapsq[i, n];
          }
          mapsqAve[n] /= rowLength-1;
      }
    }

    private void SaveTextAve(int t)
    {
      for (int n = 0; n < columnLength-1; n++)
      {
          if(n==0){if(texts.saveText("/Resources/TextFileAve.txt", "{ {")){}else{Debug.Log("Error;");}}
          if(texts.saveText("/Resources/TextFileAve.txt", mapsqAve[n].ToString() + "f")){}else{Debug.Log("Error;");}
          if(t <= 3 && (n+1) % 16 == 0){if(texts.saveText("/Resources/TextFileAve.txt", "} },\n{ {")){}else{Debug.Log("Error;");}}
          else if(t <= 3 && (n+1) % 4 == 0){if(texts.saveText("/Resources/TextFileAve.txt", "}, {")){}else{Debug.Log("Error;");}}
          else if(t <= 3){if(texts.saveText("/Resources/TextFileAve.txt", ", ")){}else{Debug.Log("Error;");}}
          if(t >= 4 && (n+1) % 9 == 0){if(texts.saveText("/Resources/TextFileAve.txt", "} },\n{ {")){}else{Debug.Log("Error;");}}
          else if(t >= 4 && (n+1) % 3 == 0){if(texts.saveText("/Resources/TextFileAve.txt", "}, {")){}else{Debug.Log("Error;");}}
          else if(t >= 4){if(texts.saveText("/Resources/TextFileAve.txt", ", ")){}else{Debug.Log("Error;");}}
          if(t <= 3 && (n+1) % 64 == 0){if(texts.saveText("/Resources/TextFileAve.txt", "\n\n\n")){}else{Debug.Log("Error;");}}
          if(t >= 4 && (n+1) % 81 == 0){if(texts.saveText("/Resources/TextFileAve.txt", "\n\n\n")){}else{Debug.Log("Error;");}}
      }
    }*/
}
