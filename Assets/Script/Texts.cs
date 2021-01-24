using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/*==============================================================*
 * TextFileMan : Assetsフォルダ以下で文字列データ管理
 *==============================================================*/
public class Texts : MonoBehaviour {

    /*----------------------------------------------------------*
     * saveText : Assetsフォルダ以下のpathのファイルにtextの内容を保存
     *       in : string path
     *          : string text
     *      out : bool
     *----------------------------------------------------------*/
    public bool saveText(string path, string text)
    {
        //ストリームライターwriterに書き込む
        try {
            using (StreamWriter writer = new StreamWriter(Application.dataPath + path,true)){
                writer.Write(text);
                writer.Flush();
                writer.Close();
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    public bool deleteText(string path)
    {
        //ストリームライターwriterに書き込む
        try {
            using (StreamWriter writer = new StreamWriter(Application.dataPath + path,false)){
                writer.Write("");
                writer.Flush();
                writer.Close();
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    /*----------------------------------------------------------*
     * saveText : Assetsフォルダ以下のpathのファイルからtextの内容を取得
     *       in : string path
     *      out : string
     *----------------------------------------------------------*/
    public string readText(string path)
    {
        //ストリームリーダーsrに読み込む
        string strStream = "";
        try {
            //※Application.dataPathはプロジェクトデータのAssetフォルダまでのアクセスパスのこと,
            using (StreamReader sr = new StreamReader(Application.dataPath + path)){
                //ストリームリーダーをstringに変換
                strStream = sr.ReadToEnd();
                sr.Close();
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
        }

        return strStream;
    }
}
