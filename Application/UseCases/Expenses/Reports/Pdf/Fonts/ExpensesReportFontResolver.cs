using System.Reflection;
using Exception.ExceptionsBase;
using PdfSharp.Fonts;

namespace Application.UseCases.Expenses.Reports.Pdf.Fonts;

public class ExpensesReportFontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName) ?? ReadFontFile(FontHelper.DEFAULT_FONT);

        var length = (int)stream!.Length;
        var data = new byte[length];
        
        stream.Read(buffer: data, offset:0, count: length);
        
        return data;
    }
    
    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName, bold, italic);
    }

    private Stream? ReadFontFile(string faceName)
    {
        var asssembly = Assembly.GetExecutingAssembly();

        return asssembly.GetManifestResourceStream(
            $"Application.UseCases.Expenses.Reports.Pdf.Fonts.{faceName}.ttf");
    }
}