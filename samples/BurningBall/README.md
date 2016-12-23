# 燃えるトーチ

キューブ2つと炎のパーティクルを組み合わせてトーチを作り，プレハブ化する。
プレハブにAudioSourceを付けて炎が燃える音を表現し，
Rigidbodyを付けて初速を与え投げたように見せる。

```csharp
var clone = Instantiate(toach, transform.position, transform.rotation);

// ランダムに初速を与える
clone.GetComponent<Rigidbody>().velocity = new Vector3(
    Random.Range(-10.0f, 10.0f), Random.Range(0.0f, 24.0f), Random.Range(10.0f, 100.0f));
```

## 参考リンク
- [【Unity】 Rigidbodyの移動方法](http://www.f-sp.com/entry/2016/08/16/211214)
