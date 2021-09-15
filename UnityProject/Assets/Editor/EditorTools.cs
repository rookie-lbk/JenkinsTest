using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTools
{

    [MenuItem("Tools/BuildAB", false, 1)]
    static void BuildAB()
    {
        BuildABCommand.OnStartBuildAB();
    }

}
