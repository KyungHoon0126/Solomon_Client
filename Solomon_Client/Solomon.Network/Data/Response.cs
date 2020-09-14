using System;

namespace Solomon.Network.Data
{
    public class Response<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static implicit operator Response<T>(Response<string> v)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class Nothing
    {
        public static Nothing AtAll => null;
    }
}
