using Application.UseCases.Expenses.Reports.Pdf.Fonts;
using Domain.Reports;
using Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
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

        var document = CreateDocument(month);
        var page = CreatePage(document);
        
        var paragraph = page.AddParagraph();
        var title = string.Format(ResourcesReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));
        
        paragraph.AddFormattedText(title, new Font( name: FontHelper.RALEWAY_REGULAR, size: 15));
        
        paragraph.AddLineBreak();

        var totalExpensesAmount = expenses.Sum(expense => expense.Amount);
        paragraph.AddFormattedText(
            $"{totalExpensesAmount} {ResourcesReportGenerationMessages.CURRENCY_SYMBOL}",
            new Font(FontHelper.WORKSANS_BLACK, size: 50));
        
        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();

        document.Info.Title = $"{ResourcesReportGenerationMessages.EXPENSES_FOR} {month:Y}";
        document.Info.Author = "CashFlow";
        
        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;
        
        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;
        
        return section;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer()
        {
            Document = document
        };
        
        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}