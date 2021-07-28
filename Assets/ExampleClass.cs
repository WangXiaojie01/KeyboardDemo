using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    public string stringToEdit = "Hello World";
    private TouchScreenKeyboard keyboard;
    private string rectStr = "(0, 0, 0, 0)";
    private string androidData = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rect rect = GetKeyboardRec();
        rectStr = "(" + rect.x + ", " + rect.y + ", " + rect.width + ", " + rect.height + ")";
#if UNITY_ANDROID
        rectStr = rectStr + androidData;
#endif
        /*
#if UNITY_IOS
        if (TouchScreenKeyboard.visible)
        {
            Rect rect = TouchScreenKeyboard.area;
            rectStr = "(" + rect.x + ", " + rect.y + ", " + rect.width + ", " + rect.height + ")";
        }
        else
        {
            rectStr = "(0, 0, 0, 0)";
        }
#elif UNITY_ANDROID

        rectStr = "(0, " + GetKeyboardHeight() + ", 0, 0)";
#else
        rectStr = "(0, 0, 0, 0)";
#endif*/
    }

    public Rect GetKeyboardRec()
    {
#if UNITY_ANDROID
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
            var view = unityPlayer.Call<AndroidJavaObject>("getView");
            var dialog = unityPlayer.Get<AndroidJavaObject>("b");

            if (view == null || dialog == null)
            {
                if (null == view)
                {
                    androidData = androidData + "view is null; ";
                }
                if (null == dialog)
                {
                    androidData = androidData + "dialog is null; ";
                }
                return new Rect(0, 0, 0, 0);
            }

            var decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");
            if (null == decorView)
            {
                androidData = androidData + "decorView is null; ";
                return new Rect(0, 0, 0, 0);
            }

            int decorHeight = decorView.Call<int>("getHeight");
            int decorBottom = decorView.Call<int>("getBottom");
            int decorWidth = decorView.Call<int>("getWidth");

            androidData = androidData + "decor height is " + decorHeight + "; bottom is " + decorBottom + "; width is " + decorWidth +"; ";

            using (var rect = new AndroidJavaObject("android.graphics.Rect"))
            {
                view.Call("getWindowVisibleDisplayFrame", rect);
                int rectBottom = rect.Get<int>("bottom");
                androidData = androidData + "rectBootom is " + rectBottom + "; ";

                Rect keyboardRect = new Rect();
                keyboardRect.x = 0;
                keyboardRect.y = decorHeight - rectBottom > 0 ? rectBottom : 0;
                keyboardRect.height = keyboardRect.y > 0 ? (decorBottom - keyboardRect.y) : 0;
                keyboardRect.width = keyboardRect.y > 0 ? decorWidth : 0;
                return keyboardRect;
            }
        }
        return new Rect(0, 0, 0, 0);
#elif UNITY_IOS
        if (TouchScreenKeyboard.visible)
        {
            return TouchScreenKeyboard.area;
        }
        else
        {
            return new Rect(0, 0, 0, 0);
        }
#else
        return new Rect(0, 0, 0, 0);
#endif
    }

        /*
            var decorHeight = 0;

        if (true)//includeInput
        {
            var decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");

            if (decorView != null)
                decorHeight = decorView.Call<int>("getHeight");
        }

        using (var rect = new AndroidJavaObject("android.graphics.Rect"))
        {
            view.Call("getWindowVisibleDisplayFrame", rect);
            return Display.main.systemHeight - rect.Call<int>("height") + decorHeight;
        }
    }
#endif
        return new Rect(0, 0, 0, 0);
    }*/

    void OnGUI()
    {
        GUIStyle txtStyle = new GUIStyle();
        txtStyle.fontSize = 25;
        stringToEdit = GUI.TextField(new Rect(10, 10, 300,50), stringToEdit, txtStyle);

        rectStr = GUI.TextField(new Rect(10, 60, 300, 50), rectStr, txtStyle);

        if (GUI.Button(new Rect(10, 110, 200, 100), "Default"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        }
        if (GUI.Button(new Rect(10, 210, 200, 100), "ASCIICapable"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.ASCIICapable);
        }
        if (GUI.Button(new Rect(10, 310, 200, 100), "Numbers and Punctuation"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumbersAndPunctuation);
        }
        if (GUI.Button(new Rect(10, 410, 200, 100), "URL"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.URL);
        }
        if (GUI.Button(new Rect(10, 510, 200, 100), "NumberPad"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
        }
        if (GUI.Button(new Rect(10, 610, 200, 100), "PhonePad"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.PhonePad);
        }
        if (GUI.Button(new Rect(10, 710, 200, 100), "NamePhonePad"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NamePhonePad);
        }
        if (GUI.Button(new Rect(10, 810, 200, 100), "EmailAddress"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.EmailAddress);
        }
        if (GUI.Button(new Rect(10, 910, 200, 100), "Social"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Social);
        }
        if (GUI.Button(new Rect(10, 1010, 200, 100), "Search"))
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Search);
        }
    }
}
