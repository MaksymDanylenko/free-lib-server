using System;

namespace FreeLibServer.Controllers.Parameters
{
    public class BookParameters : QueryStringParameters
    {
        public int MinYear { get; set; } = 0;

        public int MaxYear { get; set; } = (int)DateTime.Now.Year;
    }
}