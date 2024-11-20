using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f, yOffset;
    public Vector3 offset;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);

        // Chuyển đổi vị trí chuột sang tọa độ thế giới
        //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //// Giữ lại giá trị z của camera để không làm thay đổi chiều sâu
        //mouseWorldPosition.z = transform.position.z;

        // Di chuyển camera về phía vị trí chuột với tốc độ moveSpeed
        //transform.position = Vector3.Lerp(transform.position, mouseWorldPosition + offset, 1f * Time.deltaTime);
    }

    
}
