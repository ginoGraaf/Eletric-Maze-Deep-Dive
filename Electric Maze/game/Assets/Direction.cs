using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    N=0,E,S,W
    
};

static class DirectionExtension
{
    public static int ToBits(this Direction direction)
    {
        return 1 << (int)direction;
    }
    public static Direction Opposite(this Direction direction)
    {
        switch(direction)
        {
            case Direction.N: return Direction.S;
            case Direction.S:return Direction.N;
            case Direction.E: return Direction.W;
            case Direction.W: return Direction.E;
            default:
                throw new ArgumentOutOfRangeException(direction.ToString());
        }
    }
    public static Direction Next(this Direction direction,int delta) 
    { 
        var size =Enum.GetValues(typeof(Direction)).Length;
        int nextDir=((int)direction+(int)Mathf.Sign(delta))%size;
        if (nextDir < 0) nextDir = size + nextDir;
        return (Direction)nextDir;
    }
}
