using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Data.Repository.Interfaces.General;
using QRCoder;

namespace Business.Services
{
    /// <summary>
    /// Implementación del servicio de generación de códigos QR.
    /// Esta clase utiliza la librería QRCoder para generar imágenes QR
    /// y las almacena en Cloudinary, retornando la URL pública.
    /// </summary>
    public class QrCodeService : IQrCodeService
    {
        private readonly Cloudinary _cloudinary;

        /// <summary>
        /// Constructor que recibe la instancia configurada de Cloudinary.
        /// </summary>
        /// <param name="cloudinary">Instancia de Cloudinary registrada en DI.</param>
        public QrCodeService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        /// <summary>
        /// Genera un código QR a partir del contenido proporcionado y lo sube a Cloudinary.
        /// </summary>
        /// <param name="content">Texto o datos a codificar dentro del código QR.</param>
        /// <param name="fileNameWithoutExtension">Nombre base del archivo (sin extensión).</param>
        /// <returns>URL pública del archivo generado en Cloudinary.</returns>
        public string GenerateAndSaveQrCode(string content, string fileNameWithoutExtension)
        {
            // Generador de código QR con nivel de corrección de errores Q
            using var generator = new QRCodeGenerator();
            QRCodeData data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);

            // Crear imagen a partir del QR generado (bytes en formato PNG)
            var qrCode = new PngByteQRCode(data);
            var pngBytes = qrCode.GetGraphic(20); // 20 = píxeles por módulo

            // Construir un nombre único para el archivo
            string fileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}";

            // Subir el QR a Cloudinary usando un stream en memoria
            using var stream = new MemoryStream(pngBytes);
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                PublicId = $"qrcodes/{fileName}", // carpeta qrcodes en Cloudinary
                Overwrite = true,
                Folder = "qrcodes"
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            // Retorna la URL segura (https) para ser usada en frontend o APIs
            return uploadResult.SecureUrl.ToString();
        }
    }
}
