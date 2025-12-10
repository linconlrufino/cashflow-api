using Application.UseCases.Expenses.Reports.Pdf.Fonts;
using Domain.Repositories.Expenses;
using PdfSharp.Fonts;

namespace Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private readonly IExpensesReadOnlyRepository repository;

    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        this.repository = repository;
        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await repository.FilterByMonth(month);
        if(!expenses.Any())
            return [];

        return [];
    }
}