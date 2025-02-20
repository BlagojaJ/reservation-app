using System.Net;
using System.Text.Json.Serialization;

namespace Reservation.App.Application.Responses;

public class BaseResponse
{
    private ResponseStatus _status;
    private HttpStatusCode _code;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ResponseStatus Status
    {
        get { return _status; }
    }
    public HttpStatusCode Code
    {
        get { return _code; }
        set
        {
            _code = value;
            _status = SetResponseStatus(value);
        }
    }

    public BaseResponse()
    {
        _status = ResponseStatus.Success;
        _code = HttpStatusCode.OK;
    }

    public BaseResponse(HttpStatusCode code)
    {
        _status = SetResponseStatus(code);
        _code = code;
    }

    private ResponseStatus SetResponseStatus(HttpStatusCode code)
    {
        if (((int)code).ToString().StartsWith('2'))
        {
            return ResponseStatus.Success;
        }

        if (((int)code).ToString().StartsWith('4'))
        {
            return ResponseStatus.Fail;
        }

        return ResponseStatus.Error;
    }
}
