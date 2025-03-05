using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffectPrefab;
    private GameObject _explosionEffect;
    private int _explosionEffectCount = 5;

    private void Start()
    {
        SimplePool.PoolPreLoad(explosionEffectPrefab, _explosionEffectCount);
    }

    private void OnEnable()
    {
        BombCandy.OnPlayExplosionEffect += PlayExplosionEffect;
    }

    private void OnDisable()
    {
        BombCandy.OnPlayExplosionEffect -= PlayExplosionEffect;
    }

    private void PlayExplosionEffect(Vector3 pos)
    {
        _explosionEffect = SimplePool.Spawn(explosionEffectPrefab);
        _explosionEffect.transform.position = pos;
        _explosionEffect.GetComponent<ParticleSystem>().Play();
        DespawnEffect();
    }

    private async void DespawnEffect()
    {
        await UniTask.WaitForSeconds(1f);
        SimplePool.Despawn(_explosionEffect);
    }
}