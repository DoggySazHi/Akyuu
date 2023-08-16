using Akyuu.OBS.Models.OpCodes;

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
    
    public static async Task<RequestResponse> GetReplayBufferStatus(this OBSController controller)
    {
        return await controller.SendRequest(new Request("GetReplayBufferStatus"));
    }
    
    public static async Task<RequestResponse> ToggleReplayBuffer(this OBSController controller)
    {
        return await controller.SendRequest(new Request("ToggleReplayBuffer"));
    }
    
    public static async Task<RequestResponse> SaveReplayBuffer(this OBSController controller)
    {
        return await controller.SendRequest(new Request("SaveReplayBuffer"));
    }
    
    public static async Task<RequestResponse> GetLastReplayBufferReplay(this OBSController controller)
    {
        return await controller.SendRequest(new Request("GetLastReplayBufferReplay"));
    }
}