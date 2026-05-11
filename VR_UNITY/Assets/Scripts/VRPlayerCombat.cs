using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VRPlayerCombat : MonoBehaviour
{
    [Header("Setup UI & Camera")]
    public Transform myHead;          // Kéo Main Camera vào đây
    public Image reticleImage;       // Kéo Image vòng tròn (Filled) vào đây
    
    [Header("Combat Settings")]
    public float gazeTime = 2f;      
    public float interactionRange = 50f;
    
    private float timer;
    private bool isAwaitingAttack = false;

    void Update()
    {
        if (isAwaitingAttack) return;

        Ray ray = new Ray(myHead.position, myHead.forward);
        RaycastHit hit;

        // Vẽ tia đỏ để ông debug trong cửa sổ Scene
        Debug.DrawRay(myHead.position, myHead.forward * interactionRange, Color.red);

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                timer += Time.deltaTime;
                if (reticleImage != null)
                    reticleImage.fillAmount = timer / gazeTime;

                if (timer >= gazeTime)
                {
                    ExecuteAttack(hit.collider.gameObject);
                }
            }
            else { ResetGaze(); }
        }
        else { ResetGaze(); }
    }

    void ResetGaze()
    {
        timer = 0;
        if (reticleImage != null)
            reticleImage.fillAmount = 0;
    }

    void ExecuteAttack(GameObject enemy)
    {
        isAwaitingAttack = true;
        ResetGaze();

        Debug.Log("<color=yellow>Immortal:</color> Ta sẽ cho ngươi thấy sức mạnh thật sự!");

        // 1. Chạy Animation đấm (Nhớ đặt tên Trigger trong Animator là isPunching)
        Animator anim = enemy.GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("isPunching");

        // 2. Gọi Coroutine lao tới mặt mình
        StartCoroutine(LungeRoutine(enemy, myHead.position));
    }

    // Coroutine xử lý việc đối thủ lao tới
    private IEnumerator LungeRoutine(GameObject enemy, Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        float lungeDuration = 0.4f; // Tốc độ lao tới (càng nhỏ càng nhanh)
        Vector3 startingPos = enemy.transform.position;

        // Dừng lại trước mặt Mark 1.5m để không bị xuyên qua Camera
        Vector3 direction = (targetPosition - startingPos).normalized;
        Vector3 finalTarget = targetPosition - direction * 1.5f;
        finalTarget.y = startingPos.y; // Giữ độ cao bay lơ lửng

        while (elapsedTime < lungeDuration)
        {
            enemy.transform.position = Vector3.Lerp(startingPos, finalTarget, elapsedTime / lungeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        enemy.transform.position = finalTarget;
        
        // Rung máy khi đấm trúng
        #if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
        #endif

        yield return new WaitForSeconds(2f); // Chờ 2 giây sau cú đấm rồi cho lườm tiếp
        isAwaitingAttack = false;
    }
}