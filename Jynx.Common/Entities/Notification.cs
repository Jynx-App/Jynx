namespace Jynx.Common.Entities
{
    public class Notification : BaseEntity
    {
        public string UserId { get; set; } = "";

        public string Title { get; set; } = "";

        public string Body { get; set; } = "";

        public bool Read { get; set; }
    }
}
