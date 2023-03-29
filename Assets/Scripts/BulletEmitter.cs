using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    [SerializeField] private GameObject Bullet_Prefab;
    [SerializeField] private List<Transform> firePoints;
    [SerializeField] private float emitRate;

    private Camera mainCamera;
    private float timer;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        LookAtMouse();

        timer += Time.deltaTime;
        if (timer > emitRate)
        {
            if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < firePoints.Count; i++)
                {
                    GlobalRecycler.GetInstance<Bullet>(Bullet_Prefab).SpawnBullet(firePoints[i].position, transform.rotation);
                }

                timer = 0;
            }
        }
    }

    private void LookAtMouse()
    {
        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mouseWorldPosition - (Vector2)transform.position).normalized;
        transform.right = directionToMouse;
    }
}
