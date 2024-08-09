using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Vector3 direction;
    public Vector3 mousePosition;
    public event Action<int> Zoom;
    
    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    private void OnZoom(InputValue inputValue)
    {
        int value =  0;
        if(inputValue.Get<float>() > 0)
        {
            value = 1;
        }else if(inputValue.Get<float>() < 0)
        {
            value = -1;
        }
        Zoom(value);
    }

}
