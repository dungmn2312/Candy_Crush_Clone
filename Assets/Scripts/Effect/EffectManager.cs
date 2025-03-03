using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffectPrefab;
    private GameObject _explosionEffect;
    private int _explosionEffectCount = 5;

    private void Start()
    {
        SimplePool.PoolPreLoad(_explosionEffectPrefab, _explosionEffectCount);
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
        _explosionEffect = SimplePool.Spawn(_explosionEffectPrefab);
        _explosionEffect.transform.position = pos;
        _explosionEffect.GetComponent<ParticleSystem>().Play();
    }
}