namespace EventBus.Messages.Events;

public class IntegrationBaseEvent
	{
		public IntegrationBaseEvent() 
		{
			CorrelationId = Guid.NewGuid();
			CreationDate = DateTime.Now;
		}

		public IntegrationBaseEvent(Guid correlationId, DateTimeOffset creationDate)
		{
			CorrelationId = correlationId;
			CreationDate = creationDate;
		}

		public Guid CorrelationId { get; set; }
		public DateTimeOffset CreationDate { get; set; }
	}
