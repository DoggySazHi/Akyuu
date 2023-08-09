using Akyuu.OBS.Models.OpCodes;

namespace Akyuu.OBS.Requests;

public static class Recording
{
    public static async Task<RequestResponse> StartRecording(this OBSController controller)
    {
        return await controller.SendRequest(new Request("StartRecord"));
    }
    
    public static async Task<RequestResponse> StopRecording(this OBSController controller)
    {
        return await controller.SendRequest(new Request("StopRecord"));
    }
}