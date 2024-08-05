namespace Argo.MD.EmailSender.Messages.Commands;

public record SendEmail(
    string Recipient,
    string Subject,
    string MessageText);