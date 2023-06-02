
using SoundIOSharp;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/list", GetAudioDevices);
//app.MapGet("/run", RecordAndPlayAsync);

app.MapGet("/", () => "Hello Web World!");

await app.RunAsync();

static IEnumerable<string> GetAudioDevices()
{
    using var soundio = new SoundIO();
    soundio.Connect();
    soundio.FlushEvents();
    
    var inputDevices = Enumerable.Range(0, soundio.InputDeviceCount)
                                .Select(soundio.GetInputDevice)
                                .Where(device => device.ProbeError == 0)
                                .Select(device => device.Name);
    
    var outputDevices = Enumerable.Range(0, soundio.OutputDeviceCount)
                                    .Select(soundio.GetOutputDevice)
                                    .Where(device => device.ProbeError == 0)
                                    .Select(device => device.Name);
        
    return inputDevices.Concat(outputDevices);
}



//async Task<string> RecordAndPlayAsync()
//{
//    using var waveIn = new WasapiLoopbackCapture();
//    var waveFormat = waveIn.WaveFormat;
//    await using var outputStream = new MemoryStream();

//    waveIn.RecordingStopped += async (sender, args) =>
//    {
//        outputStream.Position = 0;

//        using var waveOut = new WaveOutEvent();
//        await using var inputStream = new MemoryStream(outputStream.ToArray());
//        var waveStream = new RawSourceWaveStream(inputStream, waveFormat);
//        waveOut.Init(waveStream);
//        waveOut.Play();

//        while (waveOut.PlaybackState == PlaybackState.Playing)
//            await Task.Delay(100);
//    };

//    waveIn.StartRecording();
//    await Task.Delay(TimeSpan.FromSeconds(10));

//    waveIn.StopRecording();

//    // making sure function is still running during play back
//    await Task.Delay(TimeSpan.FromSeconds(10));

//    return "Done";
//}


//IEnumerable<string> GetAudioDevices()
//{
//    var enumerator = new MMDeviceEnumerator();
//    var playbackDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
//                                    .Select((device, index) => $"Playback Device {index}: {device.FriendlyName}");

//    var recordingDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)
//                                     .Select((device, index) => $"Recording Device {index}: {device.FriendlyName}");

//    return playbackDevices.Concat(recordingDevices);
//}



