using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float XrotateSpeed;
    float clampX = 0;

    // Start is called before the first frame update
    void Start()
    {
       Apply();
    }

    // Update is called once per frame
    void Update()
    {
        float rotX = Input.GetAxis("Mouse Y") * XrotateSpeed * Time.deltaTime;

        clampX += -rotX;

        clampX = Mathf.Clamp(clampX, -60, 60);

        transform.eulerAngles = new Vector3(clampX, transform.eulerAngles.y, 0);
    }

    void Apply()
    {
        XrotateSpeed = GameManager.instance.ySensi * 10;
    }
}
