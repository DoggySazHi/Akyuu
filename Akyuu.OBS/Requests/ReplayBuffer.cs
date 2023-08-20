using Akyuu.OBS.Models.OpCodes;
using Newtonsoft.Json;

namespace Akyuu.OBS.Requests;

public static class ReplayBuffer
{
    public static async Task<RequestResponse> StartReplayBuffer(this OBSController controller)
    {
        return await controller.SendRequest(new Request("StartReplayBuffer"));
    }
    
    public static async Task<RequestResponse> StopReplayBuffer(this OBSController controller)
    {
        return await controller.SendRequest(new Request("StopReplayBuffer"));
    }
    
    public static async Task<RequestResponse<BufferStatusResponse>> GetReplayBufferStatus(this OBSController controller)
    {
        return await controller.SendRequest<BufferStatusResponse>(new Request("GetReplayBufferStatus"));
    }
    
    public static async Task<RequestResponse<BufferStatusResponse>> ToggleReplayBuffer(this OBSController controller)
    {
        return await controller.SendRequest<BufferStatusResponse>(new Request("ToggleReplayBuffer"));
    }
    
    public static async Task<RequestResponse> SaveReplayBuffer(this OBSController controller)
    {
        return await controller.SendRequest(new Request("SaveReplayBuffer"));
    }
    
    public static async Task<RequestResponse<LastReplayBufferResponse>> GetLastReplayBufferReplay(this OBSController controller)
    {
        return await controller.SendRequest<LastReplayBufferResponse>(new Request("GetLastReplayBufferReplay"));
    }
}

public class BufferStatusResponse
{
    [JsonProperty("outputActive")] public bool OutputActive { get; set; }
}

public class LastReplayBufferResponse
{
    [JsonProperty("savedReplayPath")] public string SavedReplayPath { get; set; } = null!;
}