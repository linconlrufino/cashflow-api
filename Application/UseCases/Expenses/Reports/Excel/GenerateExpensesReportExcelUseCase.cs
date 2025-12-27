using ClosedXML.Excel;
using Domain.Enums;
using Domain.Extensions;
using Domain.Reports;
using Domain.Repositories.Expenses;
using Domain.Services.LoggedUser;

namespace Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private readonly IExpensesReadOnlyRepository repository;
    private readonly ILoggedUser loggedUser;

    public GenerateExpensesReportExcelUseCase(
        IExpensesReadOnlyRepository repository,
        ILoggedUser loggedUser)
    {
        this.repository = repository;
        this.loggedUser = loggedUser;
    }
    
    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUserInfo = await loggedUser.Get();

        var expenses = await repository.FilterByMonth(loggedUserInfo.Id, month);

        if (!expenses.Any())
            return [];
        
        using var workbook = new XLWorkbook();
        workbook.Author = loggedUserInfo.Name;
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Calibri";
        
        var worksheet = workbook.Worksheets.Add("Expenses - " + month.ToString("MMMM"));
        
        InsertHeader(worksheet);

        var raw = 2;
        foreach (var expense in expenses)
        {           
            worksheet.Cell($"A{raw}").Value = expense.Title;
            worksheet.Cell($"B{raw}").Value = expense.Date.ToString("dd/MM/yy");
            worksheet.Cell($"C{raw}").Value = expense.PaymentType.PaymentTypeToString();
            
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
}