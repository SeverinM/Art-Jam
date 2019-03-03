using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public Prop prop;
    private Dictionary<string, int> keyCellTypeDic;
    private float counterBetweenKeys;
    string allWroteLetters;
    bool recordLetters = true;

    [SerializeField]
    UnityEngine.UI.Text text;

    void Start()
    {
        prop = GetComponent<Prop>();

        counterBetweenKeys = 0.0f;

        keyCellTypeDic = new Dictionary<string, int>();
        keyCellTypeDic["a"] = 0;
        keyCellTypeDic["z"] = 1;
        keyCellTypeDic["e"] = 2;
        keyCellTypeDic["r"] = 3;
        keyCellTypeDic["t"] = 4;
        keyCellTypeDic["y"] = 0;
        keyCellTypeDic["u"] = 1;
        keyCellTypeDic["i"] = 2;
        keyCellTypeDic["o"] = 3;
        keyCellTypeDic["p"] = 4;

        keyCellTypeDic["q"] = 0;
        keyCellTypeDic["s"] = 1;
        keyCellTypeDic["d"] = 2;
        keyCellTypeDic["f"] = 3;
        keyCellTypeDic["g"] = 4;
        keyCellTypeDic["h"] = 0;
        keyCellTypeDic["j"] = 1;
        keyCellTypeDic["k"] = 2;
        keyCellTypeDic["l"] = 3;
        keyCellTypeDic["m"] = 4;

        keyCellTypeDic["w"] = 0;
        keyCellTypeDic["x"] = 1;
        keyCellTypeDic["c"] = 2;
        keyCellTypeDic["v"] = 3;
        keyCellTypeDic["b"] = 4;
        keyCellTypeDic["n"] = 0;

        keyCellTypeDic["space"] = 1;
    }

    private ActionsInput GetActionInputFromTime()
    {
        ActionsInput actionsInput = ActionsInput.BiggerCell;
        if (counterBetweenKeys < 0.5f)
        {
            actionsInput = ActionsInput.BiggerCell;
        }
        else if (counterBetweenKeys < 1.0f)
        {
            actionsInput = ActionsInput.Copy;
        }
        else if (counterBetweenKeys < 1.5f)
        {
            actionsInput = ActionsInput.ModifyShape;
        }
        else if (counterBetweenKeys < 2.0f)
        {
            actionsInput = ActionsInput.SetColorPerType;
        }
        return actionsInput;
    }

    void Update()
    {
        if (!recordLetters && GameObject.FindObjectOfType<CanvasGroup>().alpha < 1)
        {
            GameObject.FindObjectOfType<CanvasGroup>().alpha += Time.deltaTime;
        }

        if (!recordLetters) return;

        counterBetweenKeys += Time.deltaTime;

        //////  A  //////
        if (Input.GetKeyUp("a"))
        {
            AkSoundEngine.PostEvent("Play_Mic_01", Camera.main.gameObject);
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["a"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "a";
        }
        //////  Z  //////
        if (Input.GetKeyUp("z"))
        {
            AkSoundEngine.PostEvent("Play_Mic_02", Camera.main.gameObject);
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["z"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "z";
        }
        //////  E  //////
        if (Input.GetKeyUp("e"))
        {
            AkSoundEngine.PostEvent("Play_Mic_03", Camera.main.gameObject);
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["e"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "e";
        }
        //////  R  //////
        if (Input.GetKeyUp("r"))
        {
            AkSoundEngine.PostEvent("Play_Mic_04", Camera.main.gameObject);
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["r"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "r";
        }
        //////  T  //////
        if (Input.GetKeyUp("t"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["t"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "t";
        }
        //////  Y  //////
        if (Input.GetKeyUp("y"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["y"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "y";
        }
        //////  U  //////
        if (Input.GetKeyUp("u"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["u"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "u";
        }
        //////  I  //////
        if (Input.GetKeyUp("i"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["i"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "i";
        }
        //////  O  //////
        if (Input.GetKeyUp("o"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["o"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "o";
        }
        //////  P  //////
        if (Input.GetKeyUp("p"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["p"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "p";
        }

        //////  Q  //////
        if (Input.GetKeyUp("q"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["q"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "q";
        }
        //////  S  //////
        if (Input.GetKeyUp("s"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["s"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "s";
        }
        //////  D  //////
        if (Input.GetKeyUp("d"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["d"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "d";
        }
        //////  F  //////
        if (Input.GetKeyUp("f"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["f"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "f";
        }
        //////  G  //////
        if (Input.GetKeyUp("g"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["g"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "g";
        }
        //////  H  //////
        if (Input.GetKeyUp("h"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["h"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "h";
        }
        //////  J  //////
        if (Input.GetKeyUp("j"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["j"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "j";
        }
        //////  K  //////
        if (Input.GetKeyUp("k"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["k"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "k";
        }
        //////  L  //////
        if (Input.GetKeyUp("l"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["l"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "l";
        }
        //////  M  //////
        if (Input.GetKeyUp("m"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["m"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "m";
        }

        //////  W  //////
        if (Input.GetKeyUp("w"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["w"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "w";
        }
        //////  X  //////
        if (Input.GetKeyUp("x"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["x"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "x";
        }
        //////  C  //////
        if (Input.GetKeyUp("c"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["c"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "c";
        }
        //////  V  //////
        if (Input.GetKeyUp("v"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["v"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "v";
        }
        //////  B  //////
        if (Input.GetKeyUp("b"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["b"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "b";
        }
        //////  N  //////
        if (Input.GetKeyUp("n"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["n"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "n";
        }

        //////  SPACE  //////
        if (Input.GetKeyUp("space"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["space"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += " ";
        }

        if (counterBetweenKeys > 2)
        {
            prop.InterpretInput(ActionsInput.NoInteract, 0);
        }

        text.text = allWroteLetters;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            recordLetters = false;
        }
    }

}
