import pyaudio
import io

def get_device_index(device_name):
    audio = pyaudio.PyAudio()
    device_index = None

    for i in range(audio.get_device_count()):
        info = audio.get_device_info_by_index(i)
        if info['name'] == device_name:
            device_index = info['index']
            break

    audio.terminate()
    return device_index

def record_audio(duration, device_name, sample_rate=16000, channels=2, format=pyaudio.paInt16):
    device_index = get_device_index(device_name)
    if device_index is None:
        print(f"Device '{device_name}' not found.")
        return

    chunk = 1024

    audio = pyaudio.PyAudio()

    stream = audio.open(
        format=format,
        channels=channels,
        rate=sample_rate,
        input=True,
        frames_per_buffer=chunk,
        input_device_index=device_index
    )

    print("Recording started...")

    audio_stream = io.BytesIO()

    for _ in range(int(sample_rate / chunk * duration)):
        data = stream.read(chunk)
        audio_stream.write(data)

    print("Recording finished.")

    stream.stop_stream()
    stream.close()
    audio.terminate()

    audio_data = audio_stream.getvalue()
    audio_stream.close()

    return audio_data

def play_audio(audio_data, device_name, sample_rate=16000, channels=2, format=pyaudio.paInt16):
    device_index = get_device_index(device_name)
    if device_index is None:
        print(f"Device '{device_name}' not found.")
        return

    audio = pyaudio.PyAudio()

    stream = audio.open(
        format=format,
        channels=channels,
        rate=sample_rate,
        output=True,
        output_device_index=device_index
    )

    stream.write(audio_data)

    stream.stop_stream()
    stream.close()
    audio.terminate()

if __name__ == '__main__':
    duration = 5
    device_name = "wm8960-soundcard: bcm2835-i2s-wm8960-hifi wm8960-hifi-0 (hw:1,0)"

    audio_data = record_audio(duration, device_name)
    play_audio(audio_data, device_name)
