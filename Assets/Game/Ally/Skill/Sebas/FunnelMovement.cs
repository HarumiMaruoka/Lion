using UnityEngine;

public class FunnelMovement : MonoBehaviour
{
    public Transform Center { get; set; }  // キャラクターのTransform

    private float _orbitRadius = 60f;  // ファンネルが移動する範囲の半径
    private float _moveSpeed = 0.5f;  // ファンネルの移動速度
    private float _noiseScale = 1f;  // ノイズのスケール（値を大きくすると動きが滑らかになる）
    private float _offset = 1.5f;

    private float _noiseOffsetX;
    private float _noiseOffsetY;

    void Start()
    {
        // ノイズのオフセットをランダムに設定
        _noiseOffsetX = Random.Range(0f, 100f);
        _noiseOffsetY = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Perlinノイズを使用してランダムなオフセットを生成
        float noiseX = Mathf.PerlinNoise(Time.time * _noiseScale + _noiseOffsetX, 0f) - 0.5f;
        float noiseY = Mathf.PerlinNoise(0f, Time.time * _noiseScale + _noiseOffsetY) - 0.5f;

        // 円状の範囲内での新しい位置を計算
        float angle = Mathf.Atan2(noiseY, noiseX);
        float radius = _orbitRadius * Mathf.Sqrt(noiseX * noiseX + noiseY * noiseY);
        float x = Center.position.x + Mathf.Cos(angle) * radius;
        float y = Center.position.y + Mathf.Sin(angle) * radius;

        // ファンネルの位置を更新
        Vector2 targetPosition = new Vector2(x, y) + new Vector2(_offset, _offset);
        transform.position = Vector2.Lerp(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
    }
}
