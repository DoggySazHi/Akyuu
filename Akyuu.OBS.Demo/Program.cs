using Akyuu.OBS;
using Akyuu.OBS.Requests;
using Newtonsoft.Json.Linq;

var file = File.ReadAllText("config.json");
var configJSON = JObject.Parse(file);
var config = new OBSConfiguration(
    configJSON["host"]?.ToObject<string>() ?? throw new InvalidOperationException("Host is not set in config"),
    configJSON["password"]?.ToObject<string>() ?? throw new InvalidOperationException("Password is not set in config"),
    configJSON["port"]?.ToObject<ushort>() ?? 4455);

Console.WriteLine($"Connecting to {config.Uri}");
var controller = new OBSController(config);
await controller.Start();

var loop = true;
while (loop)
{
    Console.WriteLine("Press: s to start recording, d to end recording, q to quit");
    var keys = Console.ReadKey(true);
    switch (keys.KeyChar)
    {
        case 's':
            await controller.StartRecording();
            break;
        case 'd':
            var result = await controller.StopRecording();
            if (result.ResponseData != null)
            {
                dynamic response = result.ResponseData;
                Console.WriteLine($"File saved to {response.outputPath}");
            }

            break;
        case 'q':
            loop = false;
            break;
    }
}

Console.WriteLine("Exiting");
await controller.Stop();
controller.Dispose();