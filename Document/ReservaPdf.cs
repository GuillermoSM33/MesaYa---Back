using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Globalization;
using System.IO;

namespace MesaYa.Document
{
    public class ReservaPdf : IDocument
    {
        private readonly string _contenido;
        private readonly byte[] _qrBytes;
        private readonly string _nombreUsuario;

        public ReservaPdf(string contenido, byte[] qrBytes, string nombreUsuario)
        {
            _contenido = contenido;
            _qrBytes = qrBytes;
            _nombreUsuario = nombreUsuario;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontFamily("Helvetica"));

                page.Header().Element(container => ComposeHeader(container));
                page.Content().Element(container => ComposeContent(container));
                page.Footer().AlignCenter().Text(txt =>
                {
                    txt.Span("MesaYa © ").FontColor(Colors.Grey.Lighten1);
                    txt.Span(DateTime.Now.Year.ToString()).FontColor(Colors.Grey.Lighten1);
                });
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.PaddingBottom(20).Column(column =>
            {
                column.Spacing(5);

                column.Item().AlignCenter().Text("🪑 Confirmación de Reserva")
                    .FontSize(28).Bold().FontColor(Colors.Blue.Medium);

                column.Item().AlignCenter().Text("Gracias por reservar con MesaYa")
                    .FontSize(14).FontColor(Colors.Grey.Medium);

                column.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(20);

                column.Item().Element(ComposeDetails);

                column.Item().AlignCenter().Element(ComposeQr);

                column.Item().AlignCenter().Text("Escanea el código QR para verificar tu reserva")
                    .FontSize(10).Italic().FontColor(Colors.Grey.Medium);
            });
        }

        private void ComposeDetails(IContainer container)
        {
            container.Border(1)
                     .BorderColor(Colors.Grey.Lighten2)
                     .Padding(15)
                     .Column(column =>
                     {
                         column.Spacing(10);
                         AddDetailRow(column, "📆 Fecha", FormatearFecha(ExtraerValor("Fecha")));
                         AddDetailRow(column, "👤 Usuario", _nombreUsuario);
                         AddDetailRow(column, "🪑 Mesa", ExtraerValor("MesaId"));
                         AddDetailRow(column, "👥 Personas", ExtraerValor("Personas"));
                         AddDetailRow(column, "🆔 Reserva", ExtraerValor("ReservaId"));
                     });
        }

        private void AddDetailRow(ColumnDescriptor column, string label, string value)
        {
            column.Item().Row(row =>
            {
                row.ConstantItem(100).Text(label).SemiBold().FontSize(12).FontColor(Colors.Grey.Darken2);
                row.RelativeItem().Text(value).FontSize(12).FontColor(Colors.Black);
            });
        }

        private void ComposeQr(IContainer container)
        {
            container.Border(1)
                     .BorderColor(Colors.Grey.Lighten2)
                     .Padding(15)
                     .Height(200)
                     .AlignCenter()
                     .AlignMiddle()
                     .Image(new MemoryStream(_qrBytes), ImageScaling.FitHeight);
        }

        private string ExtraerValor(string clave)
        {
            var partes = _contenido.Split('|');
            foreach (var parte in partes)
            {
                var kv = parte.Split(':');
                if (kv.Length >= 2 && kv[0].Trim().Equals(clave, StringComparison.OrdinalIgnoreCase))
                    return parte.Substring(parte.IndexOf(':') + 1).Trim();
            }
            return "-";
        }

        private string FormatearFecha(string fechaStr)
        {
            if (DateTime.TryParse(fechaStr, out DateTime fecha))
            {
                var cultura = new CultureInfo("es-MX");
                return fecha.ToString("dddd, dd MMMM yyyy HH:mm", cultura);
            }
            return fechaStr;
        }
    }
}
