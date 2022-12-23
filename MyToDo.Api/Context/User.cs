namespace MyToDo.Api.Context
{
    public class User : BaseEntity
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }

    }
}
