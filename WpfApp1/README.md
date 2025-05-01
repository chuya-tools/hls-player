# 使い方

- WebView2 を入れる(Windows11 なら標準で入っている？)
- 適当に D ドライブとかに HLS を入れる
- あとは WebView2Controller の中身を少し編集して起動すれば OK

# 調べたこと

- hls.js をアプリ内に組み込んだ
  → それだけ落として置く必要あり。Plugins/hls.js/dist にファイル群を格納する
  → ライセンス表示さえすれば組み込むことは可能。github にアップする都合上、コミット資産から消した

# 試した映像

```powershell
ffmpeg -i https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4 -c:v copy -c:a copy -f hls -hls_time 9 -hls_playlist_type vod -hls_segment_filename "video%3d.ts" video.m3u8
```

で取得した HLS
