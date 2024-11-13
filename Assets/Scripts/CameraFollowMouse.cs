using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    void Update()
    {
        // Chuyển đổi vị trí chuột sang tọa độ thế giới
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Giữ lại giá trị z của camera để không làm thay đổi chiều sâu
        mouseWorldPosition.z = transform.position.z;

        // Di chuyển camera về phía vị trí chuột với tốc độ moveSpeed
        transform.position = Vector3.Lerp(transform.position, mouseWorldPosition, 1f * Time.deltaTime);
    }
}
