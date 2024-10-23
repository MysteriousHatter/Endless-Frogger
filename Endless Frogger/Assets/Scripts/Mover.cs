// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 50f;
    [SerializeField] float HorizontalleftBoundary = -0.05f;
    [SerializeField] float HorizontalrightBoundary = 0.05f;
    List<GameObject> movableObjects = new List<GameObject>();

    float zMove;
    bool moveZ = true;

    void Update()
    {
        if (moveZ)
        {
            //Get the player Input
            float xMove = Input.GetAxisRaw("Horizontal");
            zMove = Input.GetAxisRaw("Vertical");
            Debug.Log("The Z axis " + zMove);
            //TODO Prevent the player moving backwards and moving off the sides of the tile
            zMove = Mathf.Clamp01(zMove);

            xMove = Mathf.Clamp(xMove, HorizontalleftBoundary, HorizontalrightBoundary);
            Debug.Log("The current xMove location " + xMove);

            //Calculate the scroll vector for the moveable objects
            Vector3 normalizedMoveVector = new Vector3(0f, 0, -zMove).normalized;
            Vector3 scrollVector = normalizedMoveVector * scrollSpeed * Time.deltaTime;

            //TODO - Translate (move) all moveable objects based on the scoll vector
            foreach (GameObject obj in movableObjects)
            {
                if (obj != null)
                {
                    obj.transform.Translate(scrollVector, Space.World);
                }
            }
        }
    }

    public void AddMovableObject(GameObject obj)
    {
        movableObjects.Add(obj);
    }

    public void RemoveMovableObject(GameObject obj)
    {
        if (movableObjects.Contains(obj))
        {
            movableObjects.Remove(obj);
        }
    }

    public float getZMove()
    {
        return zMove;
    }

    public void SetMoveZAxis(bool value)
    {
        moveZ = value;
    }

    public List<GameObject> GetMovabnleObjects()
    {
        return movableObjects;
    }

    public void clearMoveableObjects()
    {
        movableObjects.Clear();
    }
}
