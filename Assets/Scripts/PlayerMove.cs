using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public Transform player;
    public Transform visor;
    public Camera cam;
    public float speed = 4f;
    public float gravity = 9.8f;
    public Joystick moveStick;
    public Joystick lookStick;
    public JoyButton moveJB;
    public JoyButton lookJB;

    public float TouchSensitivity_x = 10f;
    public float TouchSensitivity_y = 10f;

    Vector3 movement = Vector3.zero;

    public PhotonView view;
    public CinemachineFreeLook brain;

    private void Start()
    {
        visor = transform.GetChild(0).GetComponent<Transform>();
        player = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        //cam = GetComponent<Camera>();
        //moveStick = GetComponentInChildren<Joystick>();
        view = GetComponent<PhotonView>();

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        if (brain != null)
            brain.LookAt = visor;
    }
    void Update()
    {
        if (view.IsMine)
        {
            movement = Vector3.zero;
            //cam.transform.LookAt(player);
            player.rotation = Quaternion.Euler(new Vector3(0, cam.transform.rotation.eulerAngles.y, 0));

            if (moveJB.Pressed)
            {
                float x = moveStick.Horizontal;
                float z = moveStick.Vertical;
                movement = cam.transform.right * x + cam.transform.forward * z;
                controller.Move(movement * speed * Time.deltaTime);
            }
            if (lookJB.Pressed)
            {
                float joyX = lookStick.Horizontal;
                float joyY = lookStick.Vertical;
                float temp = Mathf.InverseLerp(-1, 1, joyY);
                brain.m_XAxis.Value = joyX;
                brain.m_YAxis.Value = temp;
                
                Debug.Log("Y " + joyY);
                Debug.Log("X " + joyX);
                Debug.Log("temp " + temp);
            }

            /*if (Input.GetKey(KeyCode.W))
                controller.Move(player.forward * speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.A))
                controller.Move(-player.right * speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.S))
                controller.Move(-player.forward * speed * Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
                controller.Move(player.right * speed * Time.deltaTime);*/

            controller.Move(Vector3.down * gravity * Time.deltaTime);
        }
    }

    public void SetData(GameObject cameraParent)
    {
        Joystick[] joys = cameraParent.GetComponentsInChildren<Joystick>();
        foreach (Joystick joy in joys)
        {
            if (joy.name == "Move Stick")
                moveStick = joy;
            else if (joy.name == "Look Stick")
                lookStick = joy;
        }

        JoyButton[] jbs = cameraParent.GetComponentsInChildren<JoyButton>();
        foreach (JoyButton jb in jbs)
        {
            if (jb.name == "Move Stick")
                moveJB = jb;
            else if (jb.name == "Look Stick")
                lookJB = jb;
        }
        cam = cameraParent.GetComponent<Camera>();
        brain = cameraParent.GetComponentInChildren<CinemachineFreeLook>();
        brain.Follow = this.transform;
    }
}
