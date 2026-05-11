using UnityEngine;
using UnityEngine.UI;

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
        // Nếu đã kích hoạt tấn công, ngừng quét Gaze để tránh loop
        if (isAwaitingAttack) return;

        // Bắn Raycast từ giữa kính VR
        Ray ray = new Ray(myHead.position, myHead.forward);
        RaycastHit hit;

        // Vẽ tia đỏ trong Scene để ông dễ căn chỉnh vị trí Conquest
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

        Debug.Log("<color=red>Mark:</color> Damn, Conquest!");
        Debug.Log("<color=yellow>Conquest:</color> Stand ready for my arrival, worm!");

        // Chỗ này sau này ông gắn EnemyBehavior vào Conquest thì chỉ cần:
        // enemy.SendMessage("StartAttackSequence", SendMessageOptions.DontRequireReceiver);
    }
    public void DashAttack(Vector3 playerPos)
{
    // Dùng iTween hoặc đơn giản là Vector3.MoveTowards trong Coroutine
    // Để Conquest lao thẳng vào mặt người chơi
    StartCoroutine(LungeRoutine(playerPos));
}
}