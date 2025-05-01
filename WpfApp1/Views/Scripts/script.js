import Hls from '../../Plugins/hls.js/dist/hls.js';

const video = document.getElementById('video-element');
const videoSource = "https://test-streams.mux.dev/test_001/stream.m3u8";

console.log(Hls);

if (Hls.isSupported()) {
    const hls = new Hls();
    hls.loadSource(videoSrc);
    hls.attachMedia(video);
} else if (video.canPlayType('application/vnd.apple.mpegurl')) {  // ネイティブサポートブラウザ用
    video.src = videoSrc;
}