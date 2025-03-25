using QRCoder;

namespace MesaYa.Services
{
    public static class QrService
    {
        /// <summary>
        /// Genera un QR Code como imagen PNG en formato byte[] usando un renderer multiplataforma.
        /// </summary>
        /// <param name="contenido">Texto que se codificará en el QR.</param>
        /// <returns>Arreglo de bytes representando una imagen PNG.</returns>
        public static byte[] GenerarQr(string contenido)
        {
            var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);

            var qrCode = new PngByteQRCode(data);
            return qrCode.GetGraphic(20); // 20 = tamaño de pixel por módulo
        }
    }
}
