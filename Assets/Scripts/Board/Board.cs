using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private int _width;
    private int _height;
    private int[,] _boardMatrix;

    public Board()
    {
        _width = 7;
        _height = 8;
        _boardMatrix = new int[_width, _height];
    }

    public int Width
    {
        get
        {
            return _width;
        }
        set
        {
            _width = value;
        }
    }

    public int Height
    {
        get
        {
            return _height;
        }
        set
        {
            _height = value;
        }
    }

    public int[, ] GetMatrix()
    {
        return _boardMatrix;
    }
}