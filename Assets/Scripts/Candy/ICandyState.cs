using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICandyState
{
    void BeforeDestroy();
    void WhileDestroy();
    void DestroyCandy(Candy candy);
    void AfterDestroy();
}