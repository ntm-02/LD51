using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tile
{

    public enum RotationStyle
    {
        ALL,//exits on all
        NONE,//exits on none
        UPRIGHT,//exits on up/right, 4 rots in all
        UPDOWN,//exits on up/down, 2 rots in all
    };

    private Dictionary<RotationStyle, int[]> rotMap = new()
    {
        { RotationStyle.NONE, new int[] { 0 } },
        { RotationStyle.UPRIGHT, new int[] { 0, 90, 180, 270 } },
        { RotationStyle.UPDOWN, new int[] { 90 } },
    };

    public bool[] conns;

    public RotationStyle myRotStyle;
    public Tile(RotationStyle style)
    {

        switch (style)
        {
            case RotationStyle.ALL:
                conns = new bool[] { true, true, true, true };
                break;
            case RotationStyle.NONE:
                conns = new bool[] { false, false, false, false };
                break;
            case RotationStyle.UPRIGHT:
                conns = new bool[] { true, true, false, false };
                break;
            case RotationStyle.UPDOWN:
                conns = new bool[] { true, false, true, false };
                break;

        }

    }
}
