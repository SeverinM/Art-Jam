using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public Prop prop;
    private Dictionary<string, int> keyCellTypeDic;
    private float counterBetweenKeys;
    string allWroteLetters;
    bool recordLetters = true;
    bool gameStarted = false;

    public CanvasGroup canvasEndGame;
    public CanvasGroup canvasStartGame;

    public Button replayButton;

    [SerializeField]
    UnityEngine.UI.Text text;

    void Start()
    {
        AkSoundEngine.SetState("State_Amb_InGame", "Amb_InGame");

        prop = GetComponent<Prop>();

        counterBetweenKeys = 0.0f;

        replayButton.onClick.AddListener( Replay );

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

        keyCellTypeDic["space"] = 1;
    }

    void Replay()
    {
        SceneManager.LoadScene("MainScene");
    }

    private ActionsInput GetActionInputFromTime()
    {
        if(!gameStarted)
        {
            gameStarted = true;
        }

        ActionsInput actionsInput = ActionsInput.BiggerCell;
        if (counterBetweenKeys < 0.2f)
        {
            actionsInput = ActionsInput.BiggerCell;
        }
        else if (counterBetweenKeys < 0.5f)
        {
            actionsInput = ActionsInput.Copy;
        }
        else if (counterBetweenKeys < 0.7f)
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
        if(gameStarted && canvasStartGame.alpha > 0)
        {
            canvasStartGame.alpha -= Time.deltaTime * 2;
        }

        if (!recordLetters && canvasEndGame.alpha < 1)
        {
            canvasEndGame.alpha += Time.deltaTime;
        }

        if (!recordLetters) return;

        counterBetweenKeys += Time.deltaTime;

        //////  A  //////
        if (Input.GetKeyUp("a"))
        {
            prop.InterpretInput(GetActionInputFromTime(), keyCellTypeDic["a"]);
            counterBetweenKeys = 0.0f;
            allWroteLetters += "a";
        }
        //////  Z  //////
        if (Input.GetKeyUp("z"))
        {
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

        if (Input.GetKeyUp(KeyCode.Escape) && recordLetters)
        {
            recordLetters = false;
            AkSoundEngine.SetState("State_Amb_InGame", "End_AmbInGame");
        }
    }

}
