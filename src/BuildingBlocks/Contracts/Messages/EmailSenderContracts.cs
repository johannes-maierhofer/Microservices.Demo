namespace BuildingBlocks.Contracts.Messages
{
    public static class EmailSenderContracts
    {
        public class SendEmail
        {
            public string Recipient { get; set; } = string.Empty;
            public string Subject { get; set; } = string.Empty;
            public string MessageText { get; set; } = string.Empty;
        }
    }
}
