namespace MyToDo.Shared.Contact
{
    public class ApiResponse
    { 
 
        public string Message { get; set; }
        public bool Status { get; set; }   = false;
        public object Result { get; set; }

    }
    public class ApiResponse<T>
    {

        public string Message { get; set; }
        public bool Status { get; set; } = false;
        public T Result { get; set; }

    }
}
