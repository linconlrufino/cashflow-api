using Domain.Enums;
using Domain.Reports;

namespace Domain.Extensions;

public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourcesReportGenerationMessages.CASH,
            PaymentType.CreditCard => ResourcesReportGenerationMessages.CREDIT_CARD,
            PaymentType.DebitCard => ResourcesReportGenerationMessages.DEBIT_CARD,
            PaymentType.EletronicTransfer => ResourcesReportGenerationMessages.ELETRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}