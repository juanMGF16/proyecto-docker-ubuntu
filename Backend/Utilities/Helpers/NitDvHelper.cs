namespace Utilities.Helpers
{
    public static class NitHelper
    {
        public static int CalcularDV(string nitBase)
        {
            int[] factores = { 71, 67, 59, 53, 47, 43, 41, 37, 29, 23, 19, 17, 13, 7, 3 };
            int suma = 0;
            int nitLength = nitBase.Length;

            for (int i = 0; i < nitLength; i++)
            {
                int digito = int.Parse(nitBase[nitLength - 1 - i].ToString());
                suma += digito * factores[factores.Length - 1 - i];
            }

            int residuo = suma % 11;
            return residuo > 1 ? 11 - residuo : residuo;
        }
    }
}
