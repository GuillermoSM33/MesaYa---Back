using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Drawing;

namespace MesaYa.Document
{
    public class ReservaPdf : IDocument
    {
        private readonly string _contenido;
        private readonly byte[] _qrBytes;

        public ReservaPdf(string contenido, byte[] qrBytes)
        {
            _contenido = contenido;
            _qrBytes = qrBytes;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(16));

                page.Content()
                    .Column(column =>
                    {
                        column.Item().PaddingBottom(10).Text("📄 Detalle de la Reservación").Bold().FontSize(20);
                        column.Item().Text(_contenido);
                        column.Item().PaddingTop(25).Image(Image.FromStream(new MemoryStream(_qrBytes)));
                    });
            });
        }
    }
}
