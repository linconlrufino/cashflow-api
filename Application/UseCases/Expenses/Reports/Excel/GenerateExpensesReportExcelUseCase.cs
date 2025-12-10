using ClosedXML.Excel;
using Domain.Enums;
using Domain.Reports;
using Domain.Repositories.Expenses;

namespace Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private readonly IExpensesReadOnlyRepository repository;
    
    public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await repository.FilterByMonth(month);

        if (!expenses.Any())
            return [];
        
        var workbook = new XLWorkbook();
        workbook.Author = "cashflow";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Calibri";
        
        var worksheet = workbook.Worksheets.Add("Expenses - " + month.ToString("MMMM"));
        
        InsertHeader(worksheet);

        var raw = 2;
        foreach (var expense in expenses)
        {           
            worksheet.Cell($"A{raw}").Value = expense.Title;
            worksheet.Cell($"B{raw}").Value = expense.Date.ToString("dd/MM/yy");
            worksheet.Cell($"C{raw}").Value = ConvertPaymentType(expense.PaymentType);
            
            worksheet.Cell($"D{raw}").Value = expense.Amount;
            worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{ResourcesReportGenerationMessages.CURRENCY_SYMBOL} #,##0.00";
            
            worksheet.Cell($"E{raw}").Value = expense.Description;
            raw++;
        }

        worksheet.Columns().AdjustToContents();        
        
        var file = new MemoryStream();
        workbook.SaveAs(file);
        
        return file.ToArray();
    }
    
    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourcesReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourcesReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourcesReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourcesReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourcesReportGenerationMessages.DESCRIPTION;
        
        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");
        
        worksheet.Cells("A1,B1,C1,E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }

    private string ConvertPaymentType(PaymentType paymentType)
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