using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public abstract class Candy : MonoBehaviour, ICandyState
{
    public enum CandyType { None, Red, Blue, Green, Yellow, Violet }
    public enum CandyAbility { None, StripHorizontal, StripVertical, Bomb, Rainbow }

    public CandyType type;
    public CandyAbility ability;
    public int score;
    private Vector2Int _pos;

    public Vector2Int Pos { get => _pos; set => _pos = value; }

    internal bool isDestroyMain;

    public abstract void BeforeDestroy();
    public abstract void WhileDestroy();
    public abstract void DestroyCandy(Candy candy);
    public abstract void AfterDestroy();
}