
namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
        {
            Id = new Guid();
           CreateDate = DateTime.Now;
        }
        public IntegrationBaseEvent(Guid id,DateTime createDate)
        {
            Id = id;
            CreateDate = createDate;
        }
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
