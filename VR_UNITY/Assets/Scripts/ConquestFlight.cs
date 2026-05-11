using UnityEngine;

public class ConquestFlight : MonoBehaviour
{
    public float amplitude = 0.2f; // Độ cao bay lên xuống
    public float frequency = 0.8f; // Tốc độ bay
    
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Tạo hiệu ứng lơ lửng chuẩn Invincible
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        
        // Luôn nhìn về phía Camera (Người chơi)
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
    }
}