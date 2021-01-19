﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BarrelScript : Interactible
{
    private GameObject _player = null;
    private bool _isOccupied = false;
    private InputMap _inputMap;

    [SerializeField] private Transform _exitTransform;
    [SerializeField] private Camera _peekCam;

    [SerializeField] private float _mouseSensitivity = 50f;
    [SerializeField] private float _yRotClamp = 90f;


    private float _yRotation = 0f;

    public void Awake()
    {
        InitInput();
    }

    public override void Interact(GameObject playerObject)
    {
        base.Interact(playerObject);

        if (!_isOccupied)
        {
            EnterBarrel(playerObject);
        }
    }

    private void MovePlayer(GameObject playerObject, Transform destination)
    {
        playerObject.transform.position = destination.position;
        playerObject.transform.rotation = destination.rotation;
    }
    private void EnterBarrel(GameObject playerObject)
    {
        OnEnable();
        //move player character
        MovePlayer(playerObject, transform);
        _player = playerObject;
        PlayerController playerController = _player.GetComponent<PlayerController>();

        _isOccupied = true;

        //disable player control
        playerController.ToggleCharacterActive();

        playerController.SwitchActiveCam(_peekCam);
    }
    void ExitBarrel()
    {
        if (_exitTransform != null)
        {
            MovePlayer(_player, _exitTransform);

            _isOccupied = false;
            PlayerController playerController = _player.GetComponent<PlayerController>();

            playerController.ToggleCharacterActive();

            playerController.SwitchActiveCam(_peekCam);

            _player = null;
        }
        else
        {
            Debug.Log(gameObject.name + " missing exit transform!");
        }
        OnDisable();
    }

    private void CamFollowMouse(Vector2 mouseInput)
    {
        //we receive a Vector 2 where x is the horizontal and y the vertical movement of the mouse over time (delta)
        float moveX = mouseInput.x * _mouseSensitivity * Time.deltaTime;


        _yRotation = Mathf.Clamp(_yRotation + moveX, -_yRotClamp, _yRotClamp);

        _peekCam.gameObject.transform.Rotate(Vector3.up * _yRotation * Time.deltaTime);
        _peekCam.gameObject.transform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    private void InitInput()
    {
        _inputMap = new InputMap();
        _inputMap.Barrel.MouseDelta.performed += context => CamFollowMouse(context.ReadValue<Vector2>());
        _inputMap.Barrel.Leave.performed += context => ExitBarrel();
    }

    private void OnEnable()
    {
        _inputMap.Enable();
    }

    private void OnDisable()
    {
        _inputMap.Disable();
    }
}
