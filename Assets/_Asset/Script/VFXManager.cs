using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private Transform vFXContainer;
    [SerializeField] private ParticleSystem fusionExplosion;
    [SerializeField] private List<ParticleSystem> explosionObjectPool;

    private void Start()
    {
        Init();
    }

    // Hàm khởi tạo pool
    private void Init()
    {
        explosionObjectPool = new List<ParticleSystem>();
        for (int i = 0; i < 20; i++)
        {
            ParticleSystem vfxExplosion = Instantiate(fusionExplosion, vFXContainer);
            vfxExplosion.gameObject.SetActive(false);
            explosionObjectPool.Add(vfxExplosion);
        }
    }

    // Hàm lấy ParticleSystem từ pool và xóa nó khỏi list
    private ParticleSystem GetExplosion()
    {
        if (explosionObjectPool.Count > 0)
        {
            ParticleSystem explosion = explosionObjectPool[0];
            explosionObjectPool.RemoveAt(0);
            return explosion;
        }
        else
        {
            ParticleSystem vfxExplosion = Instantiate(fusionExplosion, vFXContainer);
            explosionObjectPool.Add(vfxExplosion);
            return vfxExplosion;
        }
    }

    // Hàm kích hoạt ParticleSystem
    private void ActivateParticle(ParticleSystem particle)
    {
        particle.gameObject.SetActive(true);
    }

    // Hàm trả ParticleSystem vào pool
    private void ReturnExplosion(ParticleSystem particle)
    {
        particle.Stop();
        particle.gameObject.SetActive(false);
        explosionObjectPool.Add(particle);
    }

    public void TriggerExplo(Vector3 cubePose)
    {
        StartCoroutine(Cor_TriggerExplo(cubePose));
    }
    private IEnumerator Cor_TriggerExplo(Vector3 pose)
    {
        ParticleSystem explosion = GetExplosion();
        Vector3 positionSpawn = new Vector3(pose.x, pose.y, pose.z);
        explosion.transform.SetPositionAndRotation(positionSpawn, quaternion.identity);
        ActivateParticle(explosion);
        yield return new WaitForSeconds(1.5f);
        ReturnExplosion(explosion);
    }
}
