using UnityEngine;

public class ConquestFlight : MonoBehaviour
{
    public float amplitude = 0.2f; 
    public float frequency = 0.8f; 
    public Transform visualModel; // Kéo cái Model con vào đây

    void Update()
    {
        // Chỉ di chuyển cái "vỏ" hiển thị lên xuống, không di chuyển Object cha
        if (visualModel != null)
        {
            float newY = Mathf.Sin(Time.time * frequency) * amplitude;
            visualModel.localPosition = new Vector3(0, newY, 0);
        }
        
        // Luôn nhìn về phía người chơi
        Vector3 targetDir = Camera.main.transform.position - transform.position;
        targetDir.y = 0; // Giữ cho Immortal không bị ngửa ra sau
        transform.rotation = Quaternion.LookRotation(targetDir);
    }
}