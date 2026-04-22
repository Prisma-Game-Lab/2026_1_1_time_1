using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResetTutoriais : Editor
{
    [MenuItem("Tutorial/Limpar todos os Balões")]
    public static void LimparTudo()
    {
        PlayerPrefs.DeleteKey("tutorial_visto0");
        PlayerPrefs.DeleteKey("tutorial_visto1");
        PlayerPrefs.DeleteKey("tutorial_visto2");

        PlayerPrefs.Save();

    }
}
