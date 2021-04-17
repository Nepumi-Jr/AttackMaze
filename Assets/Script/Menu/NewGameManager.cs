using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewGameManager : MonoBehaviour
{

    public Button[] sizeButton = new Button[3];
    (int, int)[] mazeSize = new (int, int)[3] { (5, 7), (7, 9), (9, 11) };
    public Color inActive;
    public Color hover;
    public Color Selected;
    int selectedSize = 0;
    bool isItemAss = true;
    MenuNSerttingNMore mm;
    


    // Start is called before the first frame update
    void Start()
    {
        doSetSize(0);
        mm = GetComponent<MenuNSerttingNMore>();
    }

    public void doSetSize(int sizeInd)
    {
        ColorBlock notSel = new ColorBlock
        {
            normalColor = inActive,
            highlightedColor = hover,
            pressedColor = Selected,
            selectedColor = Selected,
            colorMultiplier = 2f,
            fadeDuration = 0.1f

        };

        ColorBlock sel = new ColorBlock
        {
            normalColor = Selected,
            highlightedColor = Selected,
            pressedColor = Selected,
            selectedColor = Selected,
            colorMultiplier = 2f,
            fadeDuration = 0.1f
        };

        selectedSize = sizeInd;
        for(int i=0;i< sizeButton.Length; i++)
        {
            sizeButton[i].colors = notSel;
        }
        sizeButton[sizeInd].colors = sel;


    }

    public void setItemAss(bool y)
    {
        isItemAss = y;
    }

    public void startNewGame()
    {
        GameDataManager.player1Maze = new MazeManager(mazeSize[selectedSize].Item1, mazeSize[selectedSize].Item2);
        GameDataManager.player2Maze = new MazeManager(mazeSize[selectedSize].Item1, mazeSize[selectedSize].Item2);
        GameDataManager.setPhase("ConP1");
        GameDataManager.itemAss = isItemAss;
        GameDataManager.saveGame();
        mm.gameNew();
    }


}
