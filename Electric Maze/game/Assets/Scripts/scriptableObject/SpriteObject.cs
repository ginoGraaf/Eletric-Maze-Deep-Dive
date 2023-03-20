using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SpriteObject", menuName = "ScriptableObjects/SpriteObject", order = 1)]
public class SpriteObject : ScriptableObject
{
   public List<SpriteList> spriteObject;
}

[Serializable]
public class SpriteList
{
    public string spriteName = "";
    public Sprite sprite;
}
