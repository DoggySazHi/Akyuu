using Akyuu.OBS.Models.OpCodes;
using Newtonsoft.Json;

namespace Akyuu.OBS.Requests;

public static class Recording
{
    public static async Task<RequestResponse> StartRecording(this OBSController controller)
    {
        return await controller.SendRequest(new Request("StartRecord"));
    }
    
    public static async Task<RequestResponse<StopRecordResponse?>> StopRecording(this OBSController controller)
    {
        return await controller.SendRequest<StopRecordResponse?>(new Request("StopRecord"));
    }
}

public class StopRecordResponse
{
    [JsonProperty("outputPath")] public string OutputPath { get; set; } = null!;
}