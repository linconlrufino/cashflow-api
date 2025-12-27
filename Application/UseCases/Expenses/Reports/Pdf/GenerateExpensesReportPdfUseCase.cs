using System.Reflection;
using Application.UseCases.Expenses.Reports.Pdf.Colors;
using Application.UseCases.Expenses.Reports.Pdf.Fonts;
using Domain.Extensions;
using Domain.Reports;
using Domain.Repositories.Expenses;
using Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25; 
        
    private readonly IExpensesReadOnlyRepository repository;
    private readonly ILoggedUser loggedUser;

    public GenerateExpensesReportPdfUseCase(
        IExpensesReadOnlyRepository repository,
        ILoggedUser loggedUser)
    {
        this.repository = repository;
        this.loggedUser = loggedUser;
        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUserInfo = await loggedUser.Get();
        
        var expenses = await repository.FilterByMonth(loggedUserInfo.Id, month);
        if(!expenses.Any())
            return [];

        var document = CreateDocument(loggedUserInfo.Name ,month);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(loggedUserInfo.Name, page);

        var totalExpensesAmount = expenses.Sum(expense => expense.Amount);
        CreateTotalSpentSection(month, page, totalExpensesAmount);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);
            
            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            AddExpenseTitle(row.Cells[0], expense.Title);
            AddHeaderForAmount(row.Cells[3]);
            
            row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 20;
            
            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseExpenseInformation(row.Cells[1]);
            
            row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseExpenseInformation(row.Cells[2]);

            AddAmountForExpense(row.Cells[3], expense.Amount);

            if (string.IsNullOrEmpty(expense.Description) is false)
                AddExpenseDescription(table, expense.Description, row);
            
            AddWhiteSpace(table);
        }
        return RenderDocument(document);
    }

    

    private static Document CreateDocument(string userName, DateOnly month)
    {
        var document = new Document();

        document.Info.Title = $"{ResourcesReportGenerationMessages.EXPENSES_FOR} {month:Y}";
        document.Info.Author = userName;
        
        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;
        
        return document;
    }

    private static Section CreatePage(Document document)
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

    private static void CreateHeaderWithProfilePhotoAndName(string userName, Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();
        
        //Ideally, the system should be refactored to use a CDN to obtain the image.
        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "UseCases/Expenses/Reports/Pdf/Logo", "troll-round-64px.png");
        row.Cells[0].AddImage(pathFile);
        
        row.Cells[1].AddParagraph($"Hey, {userName}");
        row.Cells[1].Format.Font = new Font(name: FontHelper.RALEWAY_BLACK, size:16);
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }
    
    private static void CreateTotalSpentSection(DateOnly month, Section page,decimal totalExpensesAmount)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";
        
        var title = string.Format(ResourcesReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));
        
        paragraph.AddFormattedText(title, new Font( name: FontHelper.RALEWAY_REGULAR, size: 15));
        
        paragraph.AddLineBreak();

        paragraph.AddFormattedText(
            $"{totalExpensesAmount:f2} {ResourcesReportGenerationMessages.CURRENCY_SYMBOL}",
            new Font(FontHelper.WORKSANS_BLACK, size: 50));
    }

    private static Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();
        
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }
    
    private static void AddExpenseTitle(Cell cell, string title)
    {
        cell.AddParagraph(title);
        cell.Format.Font = new Font(name: FontHelper.RALEWAY_BLACK, size:14){Color = ColorsHelper.BLACK};
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }
    
    private static void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourcesReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font(name: FontHelper.RALEWAY_BLACK, size:14){Color = ColorsHelper.WHITE};
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private static void SetStyleBaseExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font(name: FontHelper.WORKSANS_REGULAR, size:12){Color = ColorsHelper.BLACK};
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private static void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"-{amount:f2} {ResourcesReportGenerationMessages.CURRENCY_SYMBOL}");
        cell.Format.Font = new Font(name: FontHelper.WORKSANS_REGULAR, size:14){Color = ColorsHelper.BLACK};
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;    
    }

    private static void AddExpenseDescription(Table table, string description, Row row)
    {
        var descriptionRow = table.AddRow();
        descriptionRow.Height = HEIGHT_ROW_EXPENSE_TABLE;
                
        descriptionRow.Cells[0].AddParagraph(description);
        descriptionRow.Cells[0].Format.Font = new Font(name: FontHelper.WORKSANS_REGULAR, size:10){Color = ColorsHelper.BLACK};
        descriptionRow.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
        descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
        descriptionRow.Cells[0].MergeRight = 2;
        descriptionRow.Cells[0].Format.LeftIndent = 20;

        row.Cells[3].MergeDown = 1;
    }
    
    private static void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }
    
    private static byte[] RenderDocument(Document document)
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