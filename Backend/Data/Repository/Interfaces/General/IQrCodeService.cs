using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces.General
{
    public interface IQrCodeService
    {
        /// <summary>
        /// Genera un código QR con el contenido dado y lo guarda como imagen en disco.
        /// </summary>
        /// <param name="content">Contenido que se codificará en el QR</param>
        /// <param name="fileNameWithoutExtension">Nombre base para el archivo (sin .png)</param>
        /// <returns>Ruta relativa donde se guardó el archivo QR</returns>
        string GenerateAndSaveQrCode(string content, string fileNameWithoutExtension);
    }
}
