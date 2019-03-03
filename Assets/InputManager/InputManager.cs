using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Prop prop;

    private Dictionary<string, int> keyCellTypeDic;
    
    private float counterBetweenKeys;

    void Start()
    {
        prop = GetComponent<Prop>();

        counterBetweenKeys = 0.0f;

        keyCellTypeDic = new Dictionary<string, int>();
        keyCellTypeDic["a"] = 1;
        keyCellTypeDic["z"] = 2;
        keyCellTypeDic["e"] = 0;
        keyCellTypeDic["r"] = 1;
        keyCellTypeDic["t"] = 0;
        keyCellTypeDic["y"] = 2;
        keyCellTypeDic["u"] = 3;
        keyCellTypeDic["i"] = 2;
        keyCellTypeDic["o"] = 1;
        keyCellTypeDic["p"] = 4;

        keyCellTypeDic["q"] = 4;
        keyCellTypeDic["s"] = 0;
        keyCellTypeDic["d"] = 1;
        keyCellTypeDic["f"] = 3;
        keyCellTypeDic["g"] = 4;
        keyCellTypeDic["h"] = 2;
        keyCellTypeDic["j"] = 4;
        keyCellTypeDic["k"] = 4;
        keyCellTypeDic["l"] = 3;
        keyCellTypeDic["m"] = 3;

        keyCellTypeDic["w"] = 4;
        keyCellTypeDic["x"] = 4;
        keyCellTypeDic["c"] = 3;
        keyCellTypeDic["v"] = 4;
        keyCellTypeDic["b"] = 4;
        keyCellTypeDic["n"] = 0;

        keyCellTypeDic["space"] = 0;
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
        else
        {
            actionsInput = ActionsInput.SetColorPerType;
        }
        return actionsInput;
    }

    void Update()
    {
        counterBetweenKeys += Time.deltaTime;

        //////  A  //////
        if (Input.GetKeyUp("a"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["a"]);
            counterBetweenKeys = 0.0f;
        }
        //////  Z  //////
        if (Input.GetKeyUp("z"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["z"]);
            counterBetweenKeys = 0.0f;
        }
        //////  E  //////
        if (Input.GetKeyUp("e"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["e"]);
            counterBetweenKeys = 0.0f;
        }
        //////  R  //////
        if (Input.GetKeyUp("r"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["r"]);
            counterBetweenKeys = 0.0f;
        }
        //////  T  //////
        if (Input.GetKeyUp("t"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["t"]);
            counterBetweenKeys = 0.0f;
        }
        //////  Y  //////
        if (Input.GetKeyUp("y"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["y"]);
            counterBetweenKeys = 0.0f;
        }
        //////  U  //////
        if (Input.GetKeyUp("u"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["u"]);
            counterBetweenKeys = 0.0f;
        }
        //////  I  //////
        if (Input.GetKeyUp("i"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["i"]);
            counterBetweenKeys = 0.0f;
        }
        //////  O  //////
        if (Input.GetKeyUp("o"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["o"]);
            counterBetweenKeys = 0.0f;
        }
        //////  P  //////
        if (Input.GetKeyUp("p"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["p"]);
            counterBetweenKeys = 0.0f;
        }

        //////  Q  //////
        if (Input.GetKeyUp("q"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["q"]);
            counterBetweenKeys = 0.0f;
        }
        //////  S  //////
        if (Input.GetKeyUp("s"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["s"]);
            counterBetweenKeys = 0.0f;
        }
        //////  D  //////
        if (Input.GetKeyUp("d"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["d"]);
            counterBetweenKeys = 0.0f;
        }
        //////  F  //////
        if (Input.GetKeyUp("f"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["f"]);
            counterBetweenKeys = 0.0f;
        }
        //////  G  //////
        if (Input.GetKeyUp("g"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["g"]);
            counterBetweenKeys = 0.0f;
        }
        //////  H  //////
        if (Input.GetKeyUp("h"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["h"]);
            counterBetweenKeys = 0.0f;
        }
        //////  J  //////
        if (Input.GetKeyUp("j"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["j"]);
            counterBetweenKeys = 0.0f;
        }
        //////  K  //////
        if (Input.GetKeyUp("k"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["k"]);
            counterBetweenKeys = 0.0f;
        }
        //////  L  //////
        if (Input.GetKeyUp("l"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["l"]);
            counterBetweenKeys = 0.0f;
        }
        //////  M  //////
        if (Input.GetKeyUp("m"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["m"]);
            counterBetweenKeys = 0.0f;
        }

        //////  W  //////
        if (Input.GetKeyUp("w"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["w"]);
            counterBetweenKeys = 0.0f;
        }
        //////  X  //////
        if (Input.GetKeyUp("x"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["x"]);
            counterBetweenKeys = 0.0f;
        }
        //////  C  //////
        if (Input.GetKeyUp("c"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["c"]);
            counterBetweenKeys = 0.0f;
        }
        //////  V  //////
        if (Input.GetKeyUp("v"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["v"]);
            counterBetweenKeys = 0.0f;
        }
        //////  B  //////
        if (Input.GetKeyUp("b"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["b"]);
            counterBetweenKeys = 0.0f;
        }
        //////  N  //////
        if (Input.GetKeyUp("n"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["n"]);
            counterBetweenKeys = 0.0f;
        }

        //////  SPACE  //////
        if (Input.GetKeyUp("space"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["space"]);
            counterBetweenKeys = 0.0f;
        }
    }

}
