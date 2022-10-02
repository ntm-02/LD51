using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tile : MonoBehaviour
{
    public List<GameObject> pathFromRoot = new();
    /*public enum RotationStyle
    {
        ZERO,
        ONE,
        TWO_STRAIGHT,
        TWO_BENT,
        THREE,
        FOUR
    };

    private Dictionary<RotationStyle, int[]> rotMap = new()
    {
        { RotationStyle.ZERO, new int[] { 0 } },
        { RotationStyle.ONE, new int[] { 0, 90, 180, 270 } },
        { RotationStyle.TWO_STRAIGHT, new int[] { 0, 90 } },
        { RotationStyle.TWO_BENT, new int[] { 0, 90, 180, 270 } },
        { RotationStyle.THREE, new int[] { 0, 90, 180, 270 } },
        { RotationStyle.FOUR, new int[] { 0 } },
    };


    private bool[] conns { get; }//length 4: top,right,bottom,left

    public RotationStyle myRotStyle;


    public int rotationAngle;*/

    public int height;
    public Vector2 gridPos;
    public int pathCost;
    public int travelCost;
    public bool hasPlayer = false;
    public bool hasEnemy = false;
    public GameObject collisionObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasPlayer = true;
        }

        if (collision.CompareTag("Enemy"))
        {
            hasEnemy = true;
            Debug.Log(collision.gameObject.name);
            collisionObject = collision.gameObject;
        }
    }

    public GameObject GetCollisionObject()
    {
        return collisionObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasPlayer = false;
        }

        if (collision.CompareTag("Enemy"))
        {
            hasEnemy = false;
        }
    }

    public Tile(/*RotationStyle style*/int height)
    {

        /*switch (style)
        {
            case RotationStyle.FOUR:
                conns = new bool[] { true, true, true, true };
                break;
            case RotationStyle.ZERO:
                conns = new bool[] { false, false, false, false };
                break;
            case RotationStyle.TWO_BENT:
                conns = new bool[] { true, true, false, false };
                break;
            case RotationStyle.TWO_STRAIGHT:
                conns = new bool[] { true, false, true, false };
                break;
            case RotationStyle.THREE:
                conns = new bool[] { true, true, true, false };
                break;
            case RotationStyle.ONE:
                conns = new bool[] { true, false, true, false };
                break;

        }*/

    }
}
